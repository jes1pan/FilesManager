using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace FilesManager
{
    public class FtpHelper
    {
        string ftpServerIP;

        string ftpUserID;

        string ftpPassword;

        FtpWebRequest reqFTP;

        #region 连接
        /// <summary>
        /// 连接FtpWebRequest
        /// </summary>
        /// <param name="path"></param>
        private void Connect(string path)//连接ftp
        {
            //string parsePath = path.StartsWith("ftp://") ? path : "ftp://" + path;
            // 根据uri创建FtpWebRequest对象
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
            // 指定数据传输类型
            reqFTP.UseBinary = true;
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
        }

        #endregion
        #region ftp登录信息
        /// <summary>
        /// ftp登录信息
        /// </summary>
        /// <param name="ftpServerIP">FtpIP地址</param>
        /// <param name="ftpUserID">ftp用户名</param>
        /// <param name="ftpPassword">ftp密码</param>
        public void FtpUpDown(string ftpServerIP, string ftpUserID, string ftpPassword)
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpUserID = ftpUserID;
            this.ftpPassword = ftpPassword;
        }

        public void ChangeServerIp(string ftpServerIP)
        {
            this.ftpServerIP = ftpServerIP;
        }
        #endregion
        #region 获取文件列表
        /// <summary>
        /// 上面的代码示例了如何从ftp服务器上获得文件列表
        /// </summary>
        /// <param name="path">URL路径</param>
        /// <param name="WRMethods"></param>
        /// <returns>string[] </returns>
        private string[] GetFileList(string path, string WRMethods) //内部方法
        {
            StringBuilder result = new StringBuilder();
            try
            {
                Connect(path);
                reqFTP.Method = WRMethods;
                //reqFTP.UsePassive = false;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine(); //读取下一行
                }
                if (result.Length <= 0)
                {
                    return new string[] { };
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        /// <summary>
        ///根据知道的文件路径得到文件列表
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetFileList(string path)
        {
            return GetFileList(path, WebRequestMethods.Ftp.ListDirectory);
        }
        /// <summary>
        /// 默认URl文件列表
        /// </summary>
        /// <returns></returns>
        public string[] GetFileList()
        {
            return GetFileList(ftpServerIP, WebRequestMethods.Ftp.ListDirectory);
        }
        #endregion
        #region 上传文件
        /// <summary>
        ///从ftp服务器上载文件的功能 本地->FTP
        /// </summary>
        /// <param name="filename">要上传的文件</param>
        /// <param name="path">上传的路径</param>
        /// <param name="errorinfo">返回信息</param>
        /// <returns></returns>
        public bool Upload(string path, string savePath, ref string errorinfo)
        {
            FileInfo fileInf = new FileInfo(path);
            //string uri = "ftp://" + ftpServerIP +'/'+path;
            Connect(savePath);//连接         
            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            reqFTP.KeepAlive = false;
            // 指定执行什么命令
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // 上传文件时通知服务器文件的大小
            reqFTP.ContentLength = fileInf.Length;
            // 缓冲大小设置为kb 
            int buffLength = 4096;
            byte[] buff = new byte[buffLength];
            int contentLen;
            // 打开一个文件流(System.IO.FileStream) 去读上传的文件
            FileStream fs = fileInf.OpenRead();
            try
            {
                // 把上传的文件写入流
                Stream strm = reqFTP.GetRequestStream();
                // 每次读文件流的kb
                contentLen = fs.Read(buff, 0, buffLength);
                // 流内容没有结束
                while (contentLen != 0)
                {
                    // 把内容从file stream 写入upload stream 
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // 关闭两个流
                strm.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                errorinfo += string.Format("因{0},无法完成上传", ex.Message);
                ErrorLogRecord(ex.Message, path, savePath, "保存文件");
                return false;
            }
        }

        #endregion
        #region 续传文件
        /// <summary>
        /// 续传文件
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="size">文件的大小</param>
        /// <param name="path">路径</param>
        /// <param name="errorinfo">返回信息</param>
        /// <returns></returns>
        public bool Upload(string filename, long size, string path, ref string errorinfo)
        {
            path = path.Replace("\\", "/");
            FileInfo fileInf = new FileInfo(filename);
            //string uri = "ftp://" + path + "/" + fileInf.Name;
            //string uri = "ftp://" + path;
            Connect(path);//连接         
            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            reqFTP.KeepAlive = false;
            // 指定执行什么命令         
            reqFTP.Method = WebRequestMethods.Ftp.AppendFile;
            // 上传文件时通知服务器文件的大小
            reqFTP.ContentLength = fileInf.Length;
            // 缓冲大小设置为kb 
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            // 打开一个文件流(System.IO.FileStream) 去读上传的文件
            FileStream fs = fileInf.OpenRead();
            try
            {
                StreamReader dsad = new StreamReader(fs);
                fs.Seek(size, SeekOrigin.Begin);
                // 把上传的文件写入流
                Stream strm = reqFTP.GetRequestStream();
                // 每次读文件流的kb
                contentLen = fs.Read(buff, 0, buffLength);
                // 流内容没有结束
                while (contentLen != 0)
                {
                    // 把内容从file stream 写入upload stream 
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // 关闭两个流
                strm.Close();
                fs.Close();
                errorinfo = "完成";
                return true;
            }
            catch (Exception ex)
            {
                errorinfo = string.Format("因{0},无法完成上传", ex.Message);
                return false;
            }
        }
        #endregion
        #region 下载文件
        /// <summary>
        /// 上面的代码实现了从ftp服务器下载文件的功能 FTP->本地
        /// </summary>
        /// <param name="filePath">文件</param>
        /// <param name="fileName"></param>
        /// <param name="errorinfo"></param>
        /// <returns></returns>
        public bool Download(string ftpfilepath, string filePath, ref string errorinfo)
        {
            try
            {
                filePath = filePath.Replace("我的电脑\\", "");
                string savePath = Path.GetDirectoryName(filePath);
                ftpfilepath = ftpfilepath.Replace("\\", "/");
                Connect(ftpfilepath);//连接 
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 4096;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                FileStream outputStream = new FileStream(filePath, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                errorinfo += string.Format("因{0},无法下载", ex.Message);
                ErrorLogRecord(ex.Message, ftpfilepath, filePath, "保存文件");
                return false;
            }
        }
        #endregion
        #region FTP->FTP保存文件
        public static bool Download(FtpHelper ftpOut, FtpHelper ftpIn, ref string errInfo)
        {
            try
            {
                ftpOut.Connect(ftpOut.ftpServerIP);
                ftpIn.Connect(ftpIn.ftpServerIP);
                ftpIn.reqFTP.KeepAlive = false;
                ftpIn.reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                FtpWebResponse response = (FtpWebResponse)ftpOut.reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 4096;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                Stream strm = ftpIn.reqFTP.GetRequestStream();
                while (readCount != 0)
                {
                    strm.Write(buffer, 0, bufferSize);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                strm.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                errInfo += ex.Message;
                ftpOut.ErrorLogRecord(ex.Message, ftpOut.ftpServerIP, ftpIn.ftpServerIP, "保存文件");
                return false;
            }

        }
        #endregion
        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteFileName(string fileName)
        {
            try
            {
                FileInfo fileInf = new FileInfo(fileName);
                //string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                Connect(fileName);//连接         
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                ErrorLogRecord(ex.Message, fileName, fileName, "删除文件");
            }
        }
        #endregion
        #region 在ftp上创建目录
        /// <summary>
        /// 在ftp上创建目录
        /// </summary>
        /// <param name="dirName"></param>
        public void MakeDir(string dirName, ref string errInfo)
        {
            try
            {
                //string uri = "ftp://" + ftpServerIP + "/" + dirName;
                Connect(dirName);//连接      
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                errInfo += ex.Message;
                ErrorLogRecord(ex.Message, dirName, dirName, "创建目录");
            }
        }
        #endregion

        #region 删除ftp上目录
        /// <summary>
        /// 删除ftp上目录
        /// </summary>
        /// <param name="dirName"></param>
        public void delDir(string dirName)
        {
            try
            {
                //string uri = "ftp://" + ftpServerIP + "/" + dirName;
                Connect(dirName);//连接      
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                ErrorLogRecord(ex.Message, dirName, dirName, "删除目录");
            }
        }
        #endregion
        #region 获得ftp上文件大小
        /// <summary>
        /// 获得ftp上文件大小
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public long GetFileSize(string path)
        {
            long fileSize = 0;
            try
            {
                Connect(path);//连接      
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                fileSize = response.ContentLength;
                response.Close();
            }
            catch (Exception ex)
            {
                //ErrorLogRecord(ex.Message, path, path, "获得文件大小");
                fileSize = -1;
            }
            return fileSize;
        }
        #endregion
        #region 获得ftp上文件修改时间
        public DateTime GetFileUpdateTime(string path)
        {
            DateTime date = default;
            try
            {
                Connect(path);
                reqFTP.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                date = response.LastModified;
            }
            catch (Exception ex)
            {
                //ErrorLogRecord(ex.Message, path, path, "获得文件大小");
            }
            return date;
        }
        #endregion
        #region ftp上文件改名
        /// <summary>
        /// ftp上文件改名
        /// </summary>
        /// <param name="currentFilename"></param>
        /// <param name="newFilename"></param>
        public void Rename(string currentFilename, string newFilename)
        {
            try
            {
                FileInfo fileInf = new FileInfo(currentFilename);
                //string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                Connect(currentFilename);//连接
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                //Stream ftpStream = response.GetResponseStream();
                //ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                ErrorLogRecord(ex.Message, currentFilename, newFilename, "上传改名");
            }
        }
        #endregion

        #region 获得文件明晰
        /// <summary>
        /// 获得目录明晰
        /// </summary>
        /// <returns></returns>
        public string[] GetFilesDetailList()
        {
            return GetFileList(ftpServerIP, WebRequestMethods.Ftp.ListDirectoryDetails);
        }
        /// <summary>
        /// 获得目录明晰
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetFilesDetailList(string path)
        {
            //path = path.Replace("\\", "/");
            return GetFileList(path, WebRequestMethods.Ftp.ListDirectoryDetails);
        }
        #endregion


        private void ErrorLogRecord(string msg, string originalPath, string savePath, string remark)
        {
            string sysPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(sysPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "log.html")))
            {
                sw.BaseStream.Seek(-8, SeekOrigin.End);
                sw.Write("<tr bgcolor=\"#66cccc\"><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr></table>", msg, originalPath, savePath, remark, DateTime.Now);
            }
        }
    }
}
