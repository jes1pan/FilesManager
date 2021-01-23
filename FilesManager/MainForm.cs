using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilesManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            string sysPath = System.Environment.CurrentDirectory + "\\Logs";
            if (!Directory.Exists(sysPath))
            {
                Directory.CreateDirectory(sysPath);
            }
            string fileName = sysPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "log.html";
            if (!File.Exists(fileName))
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write("<table border=\"1\" cellspacing=\"0\" cellpadding=\"10\"><tr bgcolor=\"#33ffcc\"><td>Message</td><td>Path</td><td>SavePath</td><td>Remark</td><td>DateTime</td></tr></table>");
                }
            }
        }
        FtpHelper ftpSel;
        FtpHelper ftpSave;
        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        }
        LinkedList<string> folderList;
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //通过时间筛选
            //递归所有文件判断时间是否在这个区间
            if (string.IsNullOrEmpty(txtSelectFolder.Text))
            {
                MessageBox.Show("请选择要读取的文件夹！");
                return;
            }
            if (string.IsNullOrEmpty(txtSaveFolder.Text))
            {
                MessageBox.Show("请选择要保存的文件夹！");
                return;
            }
            lblCount.Visible = false;
            lblSync.Visible = false;
            lblPoint.Visible = false;
            folderList = new LinkedList<string>();
            dataGridView1.DataSource = null;
            lblLoading.Visible = true;

            DataTable dt = new DataTable();
            dt.Columns.Add("OriginalPath", typeof(string));
            dt.Columns.Add("SavePath", typeof(string));
            dt.Columns.Add("FileName", typeof(string));
            dt.Columns.Add("UpdateTime", typeof(string));
            dt.Columns.Add("IsSync", typeof(bool));

            Task.Factory.StartNew(() =>
            {

                if (!cbFtpSel.Checked)
                {
                    //改变列顺序
                    dt.Columns.Add("CreateTime", typeof(string)).SetOrdinal(4);
                    #region 本地读取逻辑
                    DirectoryInfo di = new DirectoryInfo(txtSelectFolder.Text.Trim());

                    DirectoryInfo[] dis = di.GetDirectories("*", SearchOption.AllDirectories);
                    var fls = di.GetFiles("*", SearchOption.AllDirectories);

                    string savePath = txtSaveFolder.Text.Replace("\\", "/") + "/";

                    foreach (var item in dis)
                    {
                        folderList.AddLast(savePath + item.FullName.Substring(txtSelectFolder.Text.Length + 1).Replace("\\", "/"));
                    }
                    //folderList.AddFirst(savePath + di.Name);

                    foreach (var fl in fls)
                    {
                        DataRow dr = dt.NewRow();
                        dr["OriginalPath"] = fl.FullName;
                        dr["SavePath"] = savePath + fl.FullName.ToString().Substring(txtSelectFolder.Text.Length + 1).Replace("\\", "/");
                        dr["FileName"] = fl.Name;
                        dr["CreateTime"] = fl.CreationTime;
                        dr["UpdateTime"] = fl.LastWriteTime;
                        dr["IsSync"] = false;
                        dt.Rows.Add(dr);
                    }
                    #endregion
                }
                else
                {
                    #region FTP
                    //ftpSel.ChangeServerIp(txtSelectFolder.Text);
                    //获取父目录
                    string parentFolder = txtSelectFolder.Text;
                    var filesArr = ftpSel.GetFilesDetailList(parentFolder);
                    //if (!cbFtpSave.Checked)
                    //    if (txtSelectFolder.Text.IndexOf('/') != -1)
                    //        folderList.AddFirst(txtSelectFolder.Text);
                    if (filesArr != null)
                    {
                        for (int i = 0; i < filesArr.Length; i++)
                        {
                            BuildData(filesArr[i], dt, parentFolder);
                        }
                    }
                    #endregion
                }
                Invoke(new Action(delegate { DataGridBind(dt); lblLoading.Visible = false; }));
            });

        }

        private void DataGridBind(DataTable dt)
        {
            //时间筛选
            string where = string.Empty;
            if (dtpStart.Value != DateTime.Parse("2021-1-1"))
            {
                where += "AND UpdateTime >= '" + dtpStart.Value + "'";
            }
            if (dtpEnd.Value != DateTime.Parse("2021-1-1"))
            {
                where += "AND UpdateTime <= '" + dtpEnd.Value + "'";
            }
            if (where != string.Empty)
            {
                var rows = dt.Select("1=1" + where);
                if (rows.Length > 0)
                {
                    DataTable dts = dt.Clone();
                    foreach (var item in rows)
                    {
                        dts.Rows.Add(item.ItemArray);
                    }
                    dt = dts;
                }
            }

            if (dt.Rows.Count > 0)
            {
                lblCount.Visible = true;
                lblCount.Text = "合计：" + dt.Rows.Count;
                //progressBar1.Maximum = dt.Rows.Count;

                dataGridView1.DataSource = dt;
                //调整宽度
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    int width = dt.Rows[0][i].ToString().Length * 6;
                    if (width > 180)
                        width = 180;
                    if (width < 60)
                        width = 60;
                    dataGridView1.Columns[i].Width = width;

                }
                dataGridView1.Columns["UpdateTime"].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
            }
        }

        private void BuildData(string fileInfo, DataTable dt, string parentPath)
        {
            string fileName = FileNameFix(fileInfo);
            if (fileInfo.Contains("<DIR>"))
            {
                parentPath = parentPath + "/" + fileName;
                var filesArr = ftpSel.GetFilesDetailList(parentPath);
                if (filesArr != null && filesArr.Length > 0)
                {
                    for (int i = 0; i < filesArr.Length; i++)
                    {
                        BuildData(filesArr[i], dt, parentPath);
                    }
                }
                folderList.AddFirst(txtSaveFolder.Text + parentPath.Substring(txtSelectFolder.Text.Length));
            }
            else
            {
                DateTime upTime = DateTime.Parse(fileInfo.Substring(0, 18), CultureInfo.GetCultureInfo("en-US").DateTimeFormat);
                DataRow dr = dt.NewRow();
                dr["OriginalPath"] = parentPath + "/" + fileName;
                string savePath = txtSaveFolder.Text + parentPath.Substring(txtSelectFolder.Text.Length) + "/" + fileName;
                dr["SavePath"] = savePath.Replace("\\", "/");
                dr["FileName"] = fileName;
                dr["UpdateTime"] = upTime;
                dr["IsSync"] = false;
                dt.Rows.Add(dr);
            }
        }

        //文件名空格处理
        private string FileNameFix(string str)
        {
            string strTemp = str.Substring(37);//专用FTP
            string fileName = strTemp.Substring(strTemp.IndexOf(' ')).Trim();
            return fileName;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (cbFtpSel.Checked)
            {
                FTPSettingForm fsf = new FTPSettingForm();
                if (fsf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtSelectFolder.Text = fsf.SelPath;
                    ftpSel = fsf.FTPObj;
                }
                return;
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(txtSelectFolder.Text))
            {
                fbd.SelectedPath = txtSelectFolder.Text;
            }
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSelectFolder.Text = fbd.SelectedPath;
            }
        }

        private void btnSaveFolder_Click(object sender, EventArgs e)
        {
            if (cbFtpSave.Checked)
            {
                FTPSettingForm fsf = new FTPSettingForm();
                if (fsf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtSaveFolder.Text = fsf.SelPath;
                    ftpSave = fsf.FTPObj;
                }
                return;
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(txtSaveFolder.Text))
            {
                fbd.SelectedPath = txtSaveFolder.Text;
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtSaveFolder.Text = fbd.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            string errInfo = string.Empty;
            //创建文件夹(包含空文件夹)
            GenerateFolder(ref errInfo);

            //创建文件
            DataTable dt = dataGridView1.DataSource as DataTable;

            if (dt != null && dt.Rows.Count > 0)
            {
                Task t = new Task(() =>
                {
                    Invoke(new Action(() =>
                    {
                        lblSync.Visible = true;
                        lblPoint.Visible = true;
                        lblPoint.Text = "0";
                    }));

                    GenerateFiles(dt, 0, ref errInfo);

                    if (errInfo != string.Empty)
                    {
                        MessageBox.Show("部分保存成功，请查看日志", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        System.Diagnostics.Process.Start(System.Environment.CurrentDirectory + "\\Logs");
                    }
                    else
                    {
                        MessageBox.Show("保存成功！");
                    }
                });

                t.Start();
            }
        }

        //生成文件夹
        private void GenerateFolder(ref string errInfo)
        {
            if (folderList != null)
            {
                foreach (var fl in folderList)
                {
                    if (cbFtpSave.Checked)
                    {
                        //ftp需逐级创建文件夹
                        string parentPath = fl.Substring(0, fl.LastIndexOf('/'));
                        string folderName = fl.Substring(fl.LastIndexOf('/') + 1);
                        if (!ftpSave.GetFileList(parentPath).Contains(folderName))
                        {
                            ftpSave.MakeDir(fl, ref errInfo);
                        }
                    }
                    else
                    {
                        //本地创建
                        if (!Directory.Exists(fl))
                        {
                            Directory.CreateDirectory(fl);
                        }
                    }
                }
            }
        }

        //生成文件
        private void GenerateFiles(DataTable dt, int count, ref string errInfo)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string savePath = dt.Rows[i]["SavePath"].ToString();
                string filePath = dt.Rows[i]["OriginalPath"].ToString();
                //FTP读取
                if (cbFtpSel.Checked)
                {
                    long fileSize = ftpSel.GetFileSize(filePath);
                    DateTime lastUpdTime = ftpSel.GetFileUpdateTime(filePath);
                    //FTP->FTP
                    if (cbFtpSave.Checked)
                    {
                        ftpSel.ChangeServerIp(filePath);
                        ftpSave.ChangeServerIp(savePath);
                        long saveSize = ftpSel.GetFileSize(savePath);
                        if (saveSize != -1)
                        {
                            DateTime saveLastUpdTime = ftpSave.GetFileUpdateTime(savePath);
                            if (saveSize == fileSize && saveLastUpdTime == lastUpdTime)
                            {
                                continue;
                            }
                        }
                        FtpHelper.Download(ftpSel, ftpSave, ref errInfo);
                    }
                    //FTP->本地
                    else
                    {
                        if (File.Exists(savePath))
                        {
                            FileInfo fi = new FileInfo(savePath);
                            if (fi.Length == fileSize && fi.LastWriteTime == lastUpdTime)
                            {
                                continue;
                            }
                            if (fi.Attributes == FileAttributes.ReadOnly)
                                fi.Attributes = FileAttributes.Normal;
                        }
                        ftpSel.Download(filePath, savePath, ref errInfo);
                    }
                }
                //本地读取
                else if (!cbFtpSel.Checked)
                {
                    FileInfo fi = new FileInfo(filePath);
                    //只读文件无法操作会抛出异常
                    var fiAttributes = fi.Attributes;
                    if (fiAttributes == FileAttributes.ReadOnly)
                        fi.Attributes = FileAttributes.Normal;

                    //本地->FTP
                    if (cbFtpSave.Checked)
                    {
                        //获得父路径 提取列表  判断是否存在  截取文件名和大小
                        long saveSize = ftpSave.GetFileSize(savePath);
                        if (saveSize != -1)//文件存在
                        {
                            DateTime lastUpdTime = ftpSave.GetFileUpdateTime(savePath);
                            if (fi.Length == saveSize && fi.LastWriteTime == lastUpdTime)
                            {
                                fi.Attributes = fiAttributes;
                                continue;
                            }
                        }
                        ftpSave.Upload(filePath, savePath, ref errInfo);
                    }
                    //本地->本地
                    else
                    {
                        if (File.Exists(savePath))
                        {
                            FileInfo newFi = new FileInfo(savePath);
                            //文件修改日期和大小相同  跳过 复原权限
                            if (newFi.Length == fi.Length && newFi.LastWriteTime == fi.LastWriteTime)
                            {
                                fi.Attributes = fiAttributes;
                                continue;
                            }
                        }
                        File.Copy(filePath, savePath, true);
                    }
                    //还原文件权限
                    fi.Attributes = fiAttributes;
                }
                if (errInfo == string.Empty)
                {
                    ++count;
                    lblPoint.Invoke(new Action(delegate
                    {
                        dataGridView1.Rows[i].Cells["IsSync"].Value = true;
                        //progressBar1.Value = count;
                        lblPoint.Text = count.ToString();
                    }));
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Parse("2021-1-1");
            dtpStart.Format = DateTimePickerFormat.Custom;
            dtpStart.CustomFormat = " ";
            dtpEnd.Value = DateTime.Parse("2021-1-1");
            dtpEnd.Format = DateTimePickerFormat.Custom;
            dtpEnd.CustomFormat = " ";
        }
        //datagridview绘制序号
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
            float x = e.RowBounds.Location.X + 20;
            if (e.RowIndex > 8)
            {
                x -= 4;
            }
            e.Graphics.DrawString(Convert.ToString(e.RowIndex + 1, CultureInfo.CurrentUICulture),
               e.InheritedRowStyle.Font, b, x, e.RowBounds.Location.Y + 4);
        }
    }
}