using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Financial.CommonLib.FileSys
{
    /// <summary>
    /// FTP客户端类
    /// 实现FTP上传与下载功能
    /// </summary>
    public class FTPClient
    {
        #region 属性

        string hostAndProt;
        /// <summary>
        /// 服务器地址和端口 localhost:2121
        /// </summary>
        public string HostAndProt
        {
            get { return hostAndProt; }
            set { hostAndProt = value; }
        }

        string ftpUserName;
        /// <summary>
        /// 登录名称
        /// </summary>
        public string FtpUserName
        {
            get { return ftpUserName; }
            set { ftpUserName = value; }
        }

        string ftpUserPwd;
        /// <summary>
        /// 密码
        /// </summary>
        public string FtpUserPwd
        {
            get { return ftpUserPwd; }
            set { ftpUserPwd = value; }
        }

        private FtpWebRequest ftpRequest = null;
        /// <summary>
        /// FTP客户端请求
        /// </summary>
        public FtpWebRequest FtpRequest
        {
            get { return ftpRequest; }
            set { ftpRequest = value; }
        }

        #endregion

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public FTPClient() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hostName">服务器地址和端口</param>
        /// <param name="UserName">登录名称</param>
        /// <param name="UserPwd">密码</param>
        public FTPClient(string hostName, string UserName, string UserPwd)
        {
            this.FtpUserName = UserName;
            this.FtpUserPwd = UserPwd;
            this.HostAndProt = hostName;
        }

        /// <summary>
        /// 连接FTP站点
        /// </summary>
        /// <returns>是否成功</returns>
        public bool GetConnectFtp()
        {
            try
            {
                if (FtpRequest != null)
                {
                    return true;
                }
                FtpWebRequest ftprequest = (FtpWebRequest)WebRequest.Create("ftp://" + this.HostAndProt);
                ftprequest.Credentials = new NetworkCredential(this.FtpUserName, this.FtpUserPwd);
                ftprequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse ftpResponse = (FtpWebResponse)ftprequest.GetResponse();
                ftpResponse.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 上传文件到FTP
        /// </summary>
        /// <param name="filename">上传文件路径</param>
        /// <param name="path">服务器存放路径</param>
        /// <param name="fname">创建新文件名</param>
        /// <returns>是否上传成功</returns>
        public bool Upload(string filename, string path, string fname)
        {
            if (path == null)
            {
                path = "";
            }
            FileInfo fileInf = new FileInfo(filename);
            int allbye = (int)fileInf.Length;
            int startbye = 0;

            string newFileName;
            switch (fileInf.Name.IndexOf("#"))
            {
                case -1:
                    newFileName = QCKG(fileInf.Name);
                    break;
                default:
                    newFileName = fileInf.Name.Replace("#", "＃");
                    newFileName = QCKG(newFileName);
                    break;
            }
            string uri;
            string strName = fname;
            string temp = "ftp://" + this.HostAndProt + "/" + path;
            if (!GetProvingFile(path, fname))
            {
                if (!MakeDir(strName, temp))
                {
                    return false;
                }
            }
            uri = temp + "/" + strName + "/" + newFileName;
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));//根据uri创建FtpWebRequest对象
            reqFTP.Credentials = new NetworkCredential(this.FtpUserName, this.FtpUserPwd);//ftp用户名和密码
            reqFTP.KeepAlive = false;//默认为true，连接不会被关闭,在一个命令之后被执行
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;//指定执行什么命令
            reqFTP.UseBinary = true;//指定数据传输类型
            reqFTP.ContentLength = fileInf.Length;//上传文件时通知服务器文件的大小
            int buffLength = 2048;//缓冲大小设置为2kb 
            byte[] buff = new byte[buffLength];
            FileStream fs = fileInf.OpenRead();//打开一个文件流 (System.IO.FileStream) 去读上传的文件
            Stream strm = null;
            try
            {
                strm = reqFTP.GetRequestStream();//把上传的文件写入流
                int contentLen = fs.Read(buff, 0, buffLength);//每次读文件流的2kb
                while (contentLen != 0)//流内容没有结束
                {
                    // 把内容从file stream 写入 upload stream 
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                    startbye += contentLen;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (strm != null)
                {
                    strm.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 将文件流直接上传到服务器
        /// </summary>
        /// <param name="oFileStream">上传的文件流</param>
        /// <param name="path">服务器路径： 比如 "info/"</param>
        /// <param name="fname">服务器文件名:比如"abcdefg.aspx"</param>
        /// <returns>是否上传成功</returns>
        public bool Upload(Stream oFileStream, string path, string fname)
        {
            if (path == null)
            {
                path = "";
            }
            if (path.StartsWith("/"))//去除路径开始的
            {
                path = path.Remove(0, 1);
            }
            long allbye = oFileStream.Length;
            int startbye = 0;

            string uri;
            string strName = fname;
            string temp = "ftp://" + this.HostAndProt + "/";
            if (path.Length != 0)
            {
                if (!IsDirectoryExists(temp + path))
                {
                    MakeDir(path, temp);
                }
            }
            uri = temp + path + fname;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));//根据uri创建FtpWebRequest对象 
            reqFTP.Credentials = new NetworkCredential(this.FtpUserName, this.FtpUserPwd);//ftp用户名和密码
            reqFTP.KeepAlive = false;//默认为true，连接不会被关闭,在一个命令之后被执行
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;//指定执行什么命令
            reqFTP.UseBinary = true;//指定数据传输类型
            reqFTP.ContentLength = oFileStream.Length;//上传文件时通知服务器文件的大小
            int buffLength = 2048;//缓冲大小设置为2kb 
            byte[] buff = new byte[buffLength];
            Stream fs = oFileStream;// 打开一个文件流 (System.IO.FileStream) 去读上传的文件
            Stream strm = null;
            try
            {
                strm = reqFTP.GetRequestStream();//把上传的文件写入流
                int contentLen = fs.Read(buff, 0, buffLength);//每次读文件流的2kb 
                while (contentLen != 0)// 流内容没有结束
                {
                    strm.Write(buff, 0, contentLen);// 把内容从file stream 写入upload stream 
                    contentLen = fs.Read(buff, 0, buffLength);
                    startbye += contentLen;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (strm != null)
                {
                    strm.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="sPath">文件夹路径</param>
        /// <returns>是否存在</returns>
        public bool IsDirectoryExists(string sPath)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP = null;
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                if (sPath.LastIndexOf("/") == sPath.Length - 1)//删除最后一个"/"
                {
                    sPath = sPath.Substring(0, sPath.Length - 1);
                }
                string dirName = sPath.Substring(sPath.LastIndexOf("/") + 1);//文件夹名称
                sPath = sPath.Substring(0, sPath.LastIndexOf("/"));//FTP路径
                if (sPath.StartsWith("ftp://"))
                {
                    sPath = sPath.Remove(0, 6);
                }
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + sPath));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(this.FtpUserName, this.FtpUserPwd);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("GB2312"));
                string line = reader.ReadLine();
                int dirPos = 0;
                string tempDirName = "";
                while (line != null && line.Length > 0)
                {
                    dirPos = line.IndexOf("<DIR>");
                    if (dirPos > 0)//Windows风格
                    {
                        tempDirName = line.Substring(dirPos + 5).Trim();
                        if (tempDirName == dirName)
                        {
                            return true;
                        }
                        line = reader.ReadLine();
                        continue;
                    }
                    if (line.Trim().Substring(0, 1).ToUpper() == "D")//Unix风格
                    {
                        tempDirName = line.Substring(54).Trim();
                        if (tempDirName != "." && tempDirName != "..")
                        {
                            if (tempDirName == dirName)
                            {
                                return true;
                            }
                        }
                        line = reader.ReadLine();
                        continue;
                    }
                    line = reader.ReadLine();
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns>是否存在</returns>
        public bool GetProvingFile(string path, string fileName)
        {
            string[] ArryName;
            if (!String.IsNullOrEmpty(path))
            {
                ArryName = GetFileList(this.HostAndProt + "/" + path.Remove(path.LastIndexOf("/")));
            }
            else
            {
                ArryName = GetFileList(this.HostAndProt);
            }
            if (ArryName != null)
            {
                int nn = 0;
                string[] b = null;
                string name = null;
                string temp = null;
                for (int i = 0; i < ArryName.Length; i++)
                {
                    b = ArryName[i].ToString().Split(' ');
                    name = b[b.Length - 1];
                    if (ArryName[i].IndexOf("DIR") != -1)
                    {
                        temp = name;
                    }
                    else
                    {
                        temp = name.Substring(name.LastIndexOf("."), name.Length - name.LastIndexOf("."));
                    }
                    if (fileName == temp)
                    {
                        nn = 1;
                        break;
                    }
                }
                if (nn > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// 获取文件夹列表
        /// </summary>
        /// <returns>文件夹列表</returns>
        public string[] GetFileList(string strpath)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + strpath));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(this.FtpUserName, this.FtpUserPwd);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("GB2312"));
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch
            {
                downloadFiles = null;
                return downloadFiles;
            }
        }

        /// <summary>
        /// 删除整个文件夹
        /// </summary>
        /// <param name="aimPath">文件夹路径</param>
        public void DeleteDir(string aimPath)
        {
            try
            {
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    aimPath += "/";
                }
                string[] fileList = GetFileList(aimPath);
                // 遍历所有的文件和目录
                if (fileList == null)
                {
                    aimPath = aimPath.Remove(aimPath.LastIndexOf("/"));
                    delDir(aimPath);
                    return;
                }
                foreach (string file in fileList)
                {
                    //先当作目录处理如果存在这个目录就递归删除该目录下面的文件,否则直接删除文件
                    string[] f = file.Split(' ');
                    string m = f[f.Length - 1];
                    if (file.IndexOf("DIR") != -1)
                    {
                        DeleteDir(aimPath + m);
                        continue;
                    }
                    DeleteFileName(m, aimPath);
                }
                //删除文件夹
                aimPath = aimPath.Remove(aimPath.LastIndexOf("/"));
                delDir(aimPath);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dirName">文件夹路径(相对FTP根目录)</param>
        public void delDir(string dirName)
        {
            try
            {
                string uri = "ftp://" + this.HostAndProt + "/" + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create("ftp://" + this.HostAndProt);
                reqFTP.Credentials = new NetworkCredential(this.FtpUserName, this.FtpUserPwd);
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;//向服务器发送删除文件夹的命令
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="path">路径</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteFileName(string fileName, string path)
        {
            FtpWebResponse response = null;
            try
            {
                string uri = "ftp://" + this.HostAndProt + "/" + path + fileName;

                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));// 根据uri创建FtpWebRequest对象
                reqFTP.UseBinary = true;// 指定数据传输类型
                reqFTP.Credentials = new NetworkCredential(this.FtpUserName, this.FtpUserPwd);// ftp用户名和密码
                reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭,在一个命令之后被执行
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;// 指定执行什么命令
                response = (FtpWebResponse)reqFTP.GetResponse();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        /// <summary>
        /// 去除空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>除去空格后的字符串</returns>
        private string QCKG(string str)
        {
            string a = "";
            byte[] array = null;
            int asciicode = -1;
            CharEnumerator CEnumerator = str.GetEnumerator();
            while (CEnumerator.MoveNext())
            {
                array = new byte[1];
                array = System.Text.Encoding.ASCII.GetBytes(CEnumerator.Current.ToString());
                asciicode = (short)(array[0]);
                if (asciicode != 32)
                {
                    a += CEnumerator.Current.ToString();
                }
            }
            return a;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirName">文件夹名称</param>
        /// <param name="ftpURI">FTP完整URI</param>
        /// <returns>是否创建成功</returns>
        public bool MakeDir(string dirName, string ftpURI)
        {
            FtpWebRequest reqFTP = null;
            FtpWebResponse response = null;
            //Stream ftpStream = null;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + dirName));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(this.FtpUserName, this.FtpUserPwd);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                response = (FtpWebResponse)reqFTP.GetResponse();
                //ftpStream = response.GetResponseStream();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                //if (ftpStream != null)
                //{
                //    ftpStream.Close();
                //}
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">下载文件夹路径</param>
        /// <param name="fileName">下载文件名</param>
        /// <param name="ftpPath">FTP路径</param>
        /// <param name="ftpFileName">FTP文件名</param>
        /// <returns>是否下载成功</returns>
        public bool Download(string filePath, string fileName, string ftpPath, string ftpFileName)
        {
            Stream ftpStream = null;
            FileStream outputStream = null;
            FtpWebResponse response = null;
            try
            {
                string uri = "ftp://" + this.HostAndProt + "/" + ftpPath + ftpFileName;
                outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(this.FtpUserName, this.FtpUserPwd);
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (ftpStream != null)
                {
                    ftpStream.Close();
                }
                if (outputStream != null)
                {
                    outputStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }
    }
}
