using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FilesManager
{
    public partial class FTPSettingForm : Form
    {
        private string address;

        public FTPSettingForm()
        {
            InitializeComponent();
            txtPassWord.KeyPress += TxtPassWord_KeyPress;

            string sysPath = System.Environment.CurrentDirectory + "\\setting.txt";
            if (File.Exists(sysPath))
            {
                using (StreamReader sr = new StreamReader(sysPath))
                {
                    var settingArr = sr.ReadToEnd().Split('#');
                    if (settingArr.Length == 3)
                    {
                        txtAddress.Text = settingArr[0];
                        txtUserName.Text = settingArr[1];
                        txtPassWord.Text = settingArr[2];
                    }
                    if (!string.IsNullOrWhiteSpace(txtAddress.Text))
                    {
                        address = txtAddress.Text;
                    }
                }
            }

        }
        private string ftpServerIp { get; set; }
        public FtpHelper FTPObj { get; set; }
        public string SelPath { get; set; }
        private bool hasCheckSel = true;
        private void TxtPassWord_KeyPress(object sender, KeyPressEventArgs e)
        {
            hasCheckSel = true;
            if (e.KeyChar == 13)
            {
                tvMenu.Nodes.Clear();
                if (!string.IsNullOrWhiteSpace(txtAddress.Text) && !string.IsNullOrWhiteSpace(txtUserName.Text) && !string.IsNullOrWhiteSpace(txtPassWord.Text))
                {
                    FTPObj = new FtpHelper();
                    ftpServerIp = txtAddress.Text.StartsWith("ftp://") ? txtAddress.Text.Substring(6).TrimEnd('/') : txtAddress.Text.TrimEnd('/');
                    FTPObj.FtpUpDown(ftpServerIp, txtUserName.Text, txtPassWord.Text);
                    var filesArr = FTPObj.GetFilesDetailList();
                    if (filesArr != null)
                    {
                        hasCheckSel = false;
                        if (filesArr.Length == 0)
                        {

                            MessageBoxExtend("此路径无文件夹", MessageBoxIcon.Information);
                            return;
                        }
                        List<TreeNode> treeNodes = new List<TreeNode>();

                        BuildTreeNode(filesArr, treeNodes, "");

                        tvMenu.Nodes.AddRange(treeNodes.ToArray());
                    }
                    else
                    {
                        MessageBoxExtend("连接失败！", MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BuildTreeNode(string[] filesArr, List<TreeNode> treeNodes, string parentPath)
        {
            for (int i = 0; i < filesArr.Length; i++)
            {
                TreeNode tn = new TreeNode();
                if (filesArr[i].Contains("<DIR>"))
                {
                    string Path = FileNameFix(filesArr[i]);
                    //string Path = filesArr[i].Substring(filesArr[i].LastIndexOf("<DIR>") + 5).Trim();
                    tn.Text = Path;
                    tn.Tag = parentPath + "/" + Path;
                    treeNodes.Add(tn);
                }
            }
        }

        private string FileNameFix(string str)
        {
            string strTemp = str.Substring(37);
            string fileName = strTemp.Substring(strTemp.IndexOf(' ')).Trim();
            return fileName;
        }
        private void IsSaveSetting()
        {
            if (address != txtAddress.Text)
            {
                if (MessageBox.Show("是否保存配置？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    string sysPath = System.Environment.CurrentDirectory + "\\setting.txt";
                    using (StreamWriter sw = new StreamWriter(sysPath, false))
                    {
                        sw.Write(txtAddress.Text + '#' + txtUserName.Text + '#' + txtPassWord.Text);
                    }
                }
            }
        }
        private void btnEnter_Click(object sender, EventArgs e)
        {
            string address = txtAddress.Text.StartsWith("ftp://") ? txtAddress.Text.Substring(txtAddress.Text.IndexOf('/') + 2) : txtAddress.Text;
            if (tvMenu.SelectedNode != null)
            {
                IsSaveSetting();
                SelPath = address + '/' + tvMenu.SelectedNode.Tag.ToString().TrimStart('/');
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                if (!hasCheckSel)
                {
                    IsSaveSetting();
                    SelPath = address;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBoxExtend("请选择文件夹", MessageBoxIcon.Error);
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void MessageBoxExtend(string content, MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Question:
                    MessageBox.Show(content, "提示", MessageBoxButtons.OK, icon);
                    break;
                case MessageBoxIcon.Error:
                    MessageBox.Show(content, "错误", MessageBoxButtons.OK, icon);
                    break;
                case MessageBoxIcon.Warning:
                    MessageBox.Show(content, "警告", MessageBoxButtons.OK, icon);
                    break;
                case MessageBoxIcon.Information:
                    MessageBox.Show(content, "信息", MessageBoxButtons.OK, icon);
                    break;
                default:
                    MessageBox.Show(content, "提示", MessageBoxButtons.OK, icon);
                    break;
            }
        }

        private void tvMenu_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tvMenu.SelectedNode != null)
            {
                e.Node.Nodes.Clear();
                var filesPath = e.Node.Tag.ToString();
                var files = FTPObj.GetFilesDetailList(filesPath);
                if (files != null)
                {
                    List<TreeNode> list = new List<TreeNode>();
                    BuildTreeNode(files, list, filesPath);
                    e.Node.Nodes.AddRange(list.ToArray());
                }
                tvMenu.SelectedNode.Toggle();
            }
        }
    }
}
