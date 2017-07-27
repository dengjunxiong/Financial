using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Financial.CommonLib.FileSys
{
    /// <summary>
    /// 文件上传类
    /// 该类实现Web文件调用FTP上传
    /// </summary>
    public class FTPFileUploader
    {
        private string _LastErrorMsg = string.Empty;
        private UploadConfig _Config = null;
        private UploadConfig _DownloadConfig = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oConfig">上传文件配置</param>
        public FTPFileUploader(UploadConfig oConfig)
        {
            _Config = oConfig;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oConfig">上传文件配置</param>
        /// <param name="dConfig">下载文件配置</param>
        public FTPFileUploader(UploadConfig oConfig, UploadConfig dConfig)
        {
            _Config = oConfig;
            _DownloadConfig = dConfig;
        }

        /// <summary>
        /// 从FTP判断文件是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public bool Exists(string path, string filename)
        {
            //创建FTP对象
            FileSys.FTPClient oFTPClient = new FileSys.FTPClient(_Config.FTPServerName + ":" + _Config.FTPServerPort, _Config.FTPUserName, _Config.FTPPwd);

            return oFTPClient.GetProvingFile(path, filename);
        }

        /// <summary>
        /// 从FTP删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="filename">文件名</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(string path, string filename)
        {
            //创建FTP对象
            FileSys.FTPClient oFTPClient = new FileSys.FTPClient(_Config.FTPServerName + ":" + _Config.FTPServerPort, _Config.FTPUserName, _Config.FTPPwd);

            return oFTPClient.DeleteFileName(filename, path);
        }

        /// <summary>
        /// 直接将Web发送上来的文件流上传到FTP
        /// </summary>
        /// <param name="oInfo">上传提交文件</param>
        /// <param name="nFileID">文件ID</param>
        /// <returns>上传结果</returns>
        public UploadResult Upload(UploadPostFileInfo oInfo, int nFileID)
        {
            UploadResult oRet = new UploadResult();

            string sFilePath = string.Empty;
            string sFileName = string.Empty;
            IList<UploadThumbResult> thumbList = null;

            //创建FTP对象
            FileSys.FTPClient oFTPClient = new FileSys.FTPClient(_Config.FTPServerName + ":" + _Config.FTPServerPort, _Config.FTPUserName, _Config.FTPPwd);

            //设置路径格式
            string sPath = string.Format("{0}{1}\\", DateTime.Now.Year, DateTime.Now.Month.ToString("00"));
            //创建目录
            PrepareDirectory(oFTPClient, sPath);

            //生成文件名
            //文件名格式：ID + GUID
            string sFileTitle = string.Format("{0}_{1}_{2}", nFileID, DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString(), (new Random().Next(1, 1000000)).ToString());

            sFileName = sFileTitle + "." + oInfo.FilExt;

            Stream oFileStream = oInfo.PostedFile.InputStream;

            //是否成功生成缩略图
            if (oInfo.FilType == eFileType.Image)
            {
                FileSys.ImageHelper oIH = new FileSys.ImageHelper();
                //生成水印
                oIH.AddWarterMark(oFileStream, _Config);

                //生成缩略图
                if (_Config.IsGenThumb)
                {
                    thumbList = new List<UploadThumbResult>();
                    IList<string> thumbWidth = StringHelper.SplitString(_Config.ThumbWidth, ",");
                    IList<string> thumbHeight = StringHelper.SplitString(_Config.ThumbHeight, ",");
                    string thumbName = "";
                    UploadThumbResult thumbResult = null;
                    for (int i = 0; i < thumbWidth.Count; i++)
                    {
                        if (thumbHeight.Count > i)//高度与宽度个数保持一致
                        {
                            if (i == 0)
                            {
                                thumbName = string.Format("{0}_thumb.{3}", sFileTitle, thumbWidth[i], thumbHeight[i], "png");
                            }
                            else
                            {
                                thumbName = string.Format("{0}_thumb_{1}_{2}.{3}", sFileTitle, thumbWidth[i], thumbHeight[i], "png");
                            }
                            thumbResult = new UploadThumbResult(thumbName, thumbWidth[i], thumbHeight[i]);
                            thumbResult.Success = oIH.GenThumbNail(oInfo.FilExt, oFileStream, new System.Drawing.Size(thumbResult.Width, thumbResult.Height), thumbResult.ThumbStream);
                            oInfo.PostedFile.InputStream.Seek(0, SeekOrigin.Begin);
                            if (thumbResult.Success)
                            {
                                thumbResult.ThumbStream.Seek(0, SeekOrigin.Begin);
                            }
                            thumbList.Add(thumbResult);
                        }
                    }
                }
            }

            sPath = sPath.Replace('\\', '/');

            //上传文件流到FTP服务器
            oRet.Success = oFTPClient.Upload(oFileStream, _Config.FTPRootPath + sPath, sFileName);
            if (oRet.Success)
            {
                oRet.ServerPath = sPath + sFileName;

                if (thumbList != null)
                {
                    foreach (UploadThumbResult item in thumbList)
                    {
                        if (item.Success)
                        {
                            oRet.ServerThumbPath = sPath + item.Name;
                            //上传缩略图流到文件服务器
                            oRet.Success = oFTPClient.Upload(item.ThumbStream, _Config.FTPRootPath + sPath, item.Name);
                        }
                    }
                }
            }
            return oRet;
        }

        /// <summary>
        /// 直接将文件流上传到FTP
        /// </summary>
        /// <param name="oFileStream">文件流</param>
        /// <param name="sExt">文件扩展名</param>
        /// <param name="nFileID">文件ID</param>
        /// <returns>上传结果</returns>
        public UploadResult Upload(Stream oFileStream, string sExt, int nFileID)
        {
            UploadResult oRet = new UploadResult();

            string sFilePath = string.Empty;
            string sFileName = string.Empty;
            IList<UploadThumbResult> thumbList = null;

            //创建FTP对象
            FileSys.FTPClient oFTPClient = new FileSys.FTPClient(_Config.FTPServerName + ":" + _Config.FTPServerPort, _Config.FTPUserName, _Config.FTPPwd);

            //设置路径格式
            string sPath = string.Format("{0}{1}\\", DateTime.Now.Year, DateTime.Now.Month.ToString("00"));
            //创建目录
            PrepareDirectory(oFTPClient, sPath);

            //生成文件名
            //文件名格式：ID + GUID
            string sFileTitle = string.Format("{0}_{1}_{2}", nFileID, DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString(), (new Random().Next(1, 1000000)).ToString());

            sFileName = sFileTitle + "." + sExt;

            //是否成功生成缩略图
            if (UploadFileInfo.GetFileType(sExt) == eFileType.Image)
            {
                FileSys.ImageHelper oIH = new FileSys.ImageHelper();
                //生成水印
                oIH.AddWarterMark(oFileStream, _Config);

                //生成缩略图
                if (_Config.IsGenThumb)
                {
                    thumbList = new List<UploadThumbResult>();
                    IList<string> thumbWidth = StringHelper.SplitString(_Config.ThumbWidth, ",");
                    IList<string> thumbHeight = StringHelper.SplitString(_Config.ThumbHeight, ",");
                    string thumbName = "";
                    UploadThumbResult thumbResult = null;
                    for (int i = 0; i < thumbWidth.Count; i++)
                    {
                        if (thumbHeight.Count > i)//高度与宽度个数保持一致
                        {
                            if (i == 0)
                            {
                                thumbName = string.Format("{0}_thumb.{3}", sFileTitle, thumbWidth[i], thumbHeight[i], "png");
                            }
                            else
                            {
                                thumbName = string.Format("{0}_thumb_{1}_{2}.{3}", sFileTitle, thumbWidth[i], thumbHeight[i], "png");
                            }
                            thumbResult = new UploadThumbResult(thumbName, thumbWidth[i], thumbHeight[i]);
                            thumbResult.Success = oIH.GenThumbNail(sExt, oFileStream, new System.Drawing.Size(thumbResult.Width, thumbResult.Height), thumbResult.ThumbStream);
                            oFileStream.Seek(0, SeekOrigin.Begin);
                            if (thumbResult.Success)
                            {
                                thumbResult.ThumbStream.Seek(0, SeekOrigin.Begin);
                            }
                            thumbList.Add(thumbResult);
                        }
                    }
                }
                sPath = sPath.Replace('\\', '/');

                //上传文件流到FTP服务器
                oRet.Success = oFTPClient.Upload(oFileStream, _Config.FTPRootPath + sPath, sFileName);
                if (oRet.Success)
                {
                    oRet.ServerPath = sPath + sFileName;

                    if (thumbList != null)
                    {
                        foreach (UploadThumbResult item in thumbList)
                        {
                            if (item.Success)
                            {
                                oRet.ServerThumbPath = sPath + item.Name;
                                //上传缩略图流到文件服务器
                                oRet.Success = oFTPClient.Upload(item.ThumbStream, _Config.FTPRootPath + sPath, item.Name);
                            }
                        }
                    }
                }
            }
            return oRet;
        }

        /// <summary>
        /// 上传多个图片到FTP
        /// </summary>
        /// <param name="imgs">图片</param>
        /// <param name="exts">扩展名</param>
        /// <param name="clientKey">客户端密匙</param>
        /// <returns>上传结果</returns>
        public IList<UploadResult> Upload(IList<byte[]> imgs, IList<string> exts, string clientKey)
        {
            string filePath = string.Empty;
            string fileFullName = string.Empty;

            //创建FTP对象
            FTPClient ftpClient = new FTPClient(_Config.FTPServerName + ":" + _Config.FTPServerPort, _Config.FTPUserName, _Config.FTPPwd);

            DateTime nowTime = DateTime.Now;

            //设置路径格式
            string dirPath = string.Format("{0}\\", nowTime.ToString("yyyyMMdd"));
            //创建目录
            PrepareDirectory(ftpClient, dirPath);
            dirPath = dirPath.Replace('\\', '/');

            IList<UploadResult> result = new List<UploadResult>();
            //生成缩略图
            for (int i = 0; i < imgs.Count; i++)
            {
                byte[] bytes = imgs[i];
                string ext = exts[i];

                Stream stream = new MemoryStream(bytes);


                //文件名格式：key_HHmmssfff_i+1
                string fileName = string.Format("{0}_{1}_{2}", clientKey, nowTime.ToString("HHmmssfff"), i + 1);

                fileFullName = fileName + "." + ext;

                IList<UploadThumbResult> thumbList = null;
                if (_Config.IsGenThumb)//是否生成缩略图
                {
                    thumbList = new List<UploadThumbResult>();
                    IList<string> thumbWidth = StringHelper.SplitString(_Config.ThumbWidth, ",");
                    IList<string> thumbHeight = StringHelper.SplitString(_Config.ThumbHeight, ",");
                    string thumbName = "";
                    UploadThumbResult thumbResult = null;
                    for (int j = 0; j < thumbWidth.Count; j++)
                    {
                        ImageHelper oIH = new ImageHelper();
                        if (thumbHeight.Count > j)//高度与宽度个数保持一致
                        {
                            if (j == 0)
                            {
                                thumbName = string.Format("{0}_thumb.{3}", fileName, thumbWidth[j], thumbHeight[j], "png");
                            }
                            else
                            {
                                thumbName = string.Format("{0}_thumb_{1}_{2}.{3}", fileName, thumbWidth[j], thumbHeight[j], "png");
                            }
                            thumbResult = new UploadThumbResult(thumbName, thumbWidth[j], thumbHeight[j]);
                            try
                            {
                                thumbResult.Success = oIH.GenThumbNail(ext, stream, new System.Drawing.Size(thumbResult.Width, thumbResult.Height), thumbResult.ThumbStream);
                            }
                            catch (Exception)
                            {
                                thumbResult.Success = false;
                            }
                            stream.Seek(0, SeekOrigin.Begin);
                            if (thumbResult.Success)
                            {
                                thumbResult.ThumbStream.Seek(0, SeekOrigin.Begin);
                                thumbList.Add(thumbResult);
                            }
                        }
                    }
                }
                //上传文件到FTP服务器
                UploadResult itemResult = new UploadResult();
                itemResult.Success = ftpClient.Upload(stream, _Config.FTPRootPath + dirPath, fileFullName);
                if (itemResult.Success)
                {
                    itemResult.ServerPath = _Config.FTPRootPath + dirPath + fileFullName;

                    if (thumbList != null)
                    {
                        bool existsThumb = false;
                        foreach (UploadThumbResult item in thumbList)
                        {
                            if (item.Success)
                            {
                                //上传缩略图流到文件服务器
                                bool uploadResult = ftpClient.Upload(item.ThumbStream, _Config.FTPRootPath + dirPath, item.Name);
                                if (uploadResult && !existsThumb)
                                {
                                    itemResult.ServerThumbPath = _Config.FTPRootPath + dirPath + item.Name;
                                    existsThumb = true;
                                }
                            }
                        }
                    }
                }
                result.Add(itemResult);
            }
            return result;
        }

        /// <summary>
        /// 下载并上传,并删除临时下载的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="ftpDownPath"></param>
        /// <param name="ftpDownFileName"></param>
        /// <param name="sExt"></param>
        /// <param name="nFileID"></param>
        /// <returns></returns>
        public UploadResult DownloadAndUpdate(string filePath, string fileName, string ftpDownPath, string ftpDownFileName, string sExt, int nFileID)
        {
            UploadResult oRet = new UploadResult();

            //创建下载FTP对象
            FileSys.FTPClient downFTPClient = new FileSys.FTPClient(_DownloadConfig.FTPServerName + ":" + _DownloadConfig.FTPServerPort, _DownloadConfig.FTPUserName, _DownloadConfig.FTPPwd);

            if (downFTPClient.Download(filePath, fileName, _DownloadConfig.FTPRootPath + ftpDownPath, ftpDownFileName))
            {
                string sFilePath = string.Empty;
                string sFileName = string.Empty;
                IList<UploadThumbResult> thumbList = null;

                //创建FTP对象
                FileSys.FTPClient oFTPClient = new FileSys.FTPClient(_Config.FTPServerName + ":" + _Config.FTPServerPort, _Config.FTPUserName, _Config.FTPPwd);

                //设置路径格式
                string sPath = string.Format("{0}{1}\\", DateTime.Now.Year, DateTime.Now.Month.ToString("00"));
                //创建目录
                PrepareDirectory(oFTPClient, sPath);

                //生成文件名
                //文件名格式：ID + GUID
                string sFileTitle = string.Format("{0}_{1}_{2}", nFileID, DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString(), (new Random().Next(1, 1000000)).ToString());

                sFileName = sFileTitle + "." + sExt;

                FileInfo fileInf = null;
                FileStream fs = null;
                try
                {
                    fileInf = new FileInfo(filePath + "\\" + fileName);
                    fs = fileInf.OpenRead();
                    //是否成功生成缩略图
                    if (UploadFileInfo.GetFileType(sExt) == eFileType.Image)
                    {
                        //生成缩略图
                        if (_Config.IsGenThumb)
                        {
                            thumbList = new List<UploadThumbResult>();
                            FileSys.ImageHelper oIH = new FileSys.ImageHelper();
                            IList<string> thumbWidth = StringHelper.SplitString(_Config.ThumbWidth, ",");
                            IList<string> thumbHeight = StringHelper.SplitString(_Config.ThumbHeight, ",");
                            string thumbName = "";
                            UploadThumbResult thumbResult = null;
                            for (int i = 0; i < thumbWidth.Count; i++)
                            {
                                if (thumbHeight.Count > i)//高度与宽度个数保持一致
                                {
                                    if (i == 0)
                                    {
                                        thumbName = string.Format("{0}_thumb.{3}", sFileTitle, thumbWidth[i], thumbHeight[i], "png");
                                    }
                                    else
                                    {
                                        thumbName = string.Format("{0}_thumb_{1}_{2}.{3}", sFileTitle, thumbWidth[i], thumbHeight[i], "png");
                                    }
                                    thumbResult = new UploadThumbResult(thumbName, thumbWidth[i], thumbHeight[i]);
                                    thumbResult.Success = oIH.GenThumbNail(sExt, fs, new System.Drawing.Size(thumbResult.Width, thumbResult.Height), thumbResult.ThumbStream);
                                    fs.Seek(0, SeekOrigin.Begin);
                                    if (thumbResult.Success)
                                    {
                                        thumbResult.ThumbStream.Seek(0, SeekOrigin.Begin);
                                    }
                                    thumbList.Add(thumbResult);
                                }
                            }
                        }
                    }

                    sPath = sPath.Replace('\\', '/');

                    //上传文件流到FTP服务器
                    oRet.Success = oFTPClient.Upload(fs, _Config.FTPRootPath + sPath, sFileName);
                    if (oRet.Success)
                    {
                        oRet.ServerPath = sPath + sFileName;

                        if (thumbList != null)
                        {
                            foreach (UploadThumbResult item in thumbList)
                            {
                                if (item.Success)
                                {
                                    oRet.ServerThumbPath = sPath + item.Name;
                                    //上传缩略图流到文件服务器
                                    oRet.Success = oFTPClient.Upload(item.ThumbStream, _Config.FTPRootPath + sPath, item.Name);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    oRet.Success = false;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fileInf.Delete();
                    }
                }
            }

            return oRet;
        }

        /// <summary>
        /// 直接将Web发送上来的文件流上传到FTP
        /// 支持eFileType.Vedio、eFileType.Flash以及eFileType.Office类型
        /// </summary>
        /// <param name="oInfo">上传提交文件</param>
        /// <param name="nFileID">文件ID</param>
        /// <returns>上传结果</returns>
        public UploadResult UploadVideo(UploadPostFileInfo oInfo, int nFileID)
        {
            UploadResult oRet = new UploadResult();
            switch (oInfo.FilType)
            {
                case eFileType.Vedio:
                case eFileType.Flash:
                    return Upload(oInfo, nFileID);
                case eFileType.Office:
                    break;
                default://其他文件
                    oRet.Message = "上传文件格式不对";
                    return oRet;
            }
            //生成文件名
            //文件名格式：ID + GUID
            string sFileTitle = string.Format("{0}_{1}_{2}", nFileID, DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString(), (new Random().Next(1, 1000000)).ToString());

            string tempFolder = Constants.TempFolder;
            string tempPath = string.Format("{0}{1}.{2}", tempFolder, sFileTitle, oInfo.FilExt);
            string tempExt = "";
            try
            {
                //保存到临时目录
                oInfo.PostedFile.SaveAs(tempPath);
            }
            catch (Exception)
            {
                oRet.Success = false;
                oRet.Message = "将offic文件保存到临时目录出错";
                return oRet;
            }

            bool toPdfResult = false;
            string error = "";
            try
            {
                tempExt = oInfo.FilExt;
                switch (oInfo.FilExt)
                {
                    case "doc":
                    case "docx":
                        toPdfResult = FileConvert.WordToPDF(out error, tempPath, string.Format("{0}{1}.{2}", tempFolder, sFileTitle, "pdf"));
                        break;
                    case "ppt":
                    case "pptx":

                        toPdfResult = FileConvert.PowerPointToPDF(out error, tempPath, string.Format("{0}{1}.{2}", tempFolder, sFileTitle, "pdf"));
                        break;
                    case "xls":
                    case "xlsx":
                        toPdfResult = FileConvert.ExcelToPDF(out error, tempPath, string.Format("{0}{1}.{2}", tempFolder, sFileTitle, "pdf"));
                        break;
                    default:
                        toPdfResult = true;//pdf文件不需要转换
                        break;
                }
                if (!toPdfResult)
                {
                    oRet.Success = false;
                    oRet.Message = "将offic文件转换为pdf文件失败(" + error + ")";
                    return oRet;
                }
            }
            catch (Exception ee)
            {
                oRet.Message = "将offic文件转换为pdf文件出错(" + ee.ToString() + ")";
                return oRet;
            }

            bool toSwfResult = false;
            try
            {
                toSwfResult = FileConvert.PDFToSWF(out error, Constants.PdfToSwfExePath, string.Format("{0}{1}.{2}", tempFolder, sFileTitle, "pdf"), string.Format("{0}{1}.{2}", tempFolder, sFileTitle, "swf"));
                if (!toSwfResult)
                {
                    oRet.Success = false;
                    oRet.Message = "将pdf文件转换为swf文件失败(" + error + ")";
                    return oRet;
                }
            }
            catch (Exception ee)
            {
                oRet.Success = false;
                oRet.Message = "将pdf文件转换为swf文件出错(" + ee.ToString() + ")";
                return oRet;
            }

            try
            {
                FileInfo delFile = null;
                //删除临时文件
                if (toPdfResult)//删除保存的Office文件
                {
                    delFile = new FileInfo(string.Format("{0}{1}.{2}", tempFolder, sFileTitle, tempExt));
                    delFile.Delete();
                }
                //删除pdf文件
                delFile = new FileInfo(string.Format("{0}{1}.{2}", tempFolder, sFileTitle, "pdf"));
                delFile.Delete();
            }
            catch (Exception)
            {

            }

            string sFilePath = string.Empty;
            string sFileName = string.Empty;
            //创建FTP对象
            FileSys.FTPClient oFTPClient = new FileSys.FTPClient(_Config.FTPServerName + ":" + _Config.FTPServerPort, _Config.FTPUserName, _Config.FTPPwd);

            //设置路径格式
            string sPath = string.Format("{0}{1}\\", DateTime.Now.Year, DateTime.Now.Month.ToString("00"));
            //创建目录
            PrepareDirectory(oFTPClient, sPath);

            sFileName = sFileTitle + "." + "swf";

            FileInfo fileInf = null;
            FileStream fs = null;
            try
            {
                fileInf = new FileInfo(string.Format("{0}{1}", tempFolder, sFileName));
                fs = fileInf.OpenRead();

                sPath = sPath.Replace('\\', '/');

                //上传文件流到FTP服务器
                oRet.Success = oFTPClient.Upload(fs, _Config.FTPRootPath + sPath, sFileName);
                if (oRet.Success)
                {
                    oRet.ServerPath = sPath + sFileName;
                }
            }
            catch (Exception ee)
            {
                oRet.Success = false;
                oRet.Message = "上传文件到服务器失败(" + ee.Message + ")";
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fileInf.Delete();
                }
            }
            return oRet;
        }

        /// <summary>
        /// 创建多级文件夹
        /// </summary>
        /// <param name="oFTPClient">FTP客户端类对象</param>
        /// <param name="sPath">路径</param>
        /// <returns>是否创建成功</returns>
        private bool PrepareDirectory(FTPClient oFTPClient, string sPath)
        {
            //确定路径格式
            string[] pathSegment = sPath.Split('\\');


            if (_Config.ServerType == FileServerTypes.FTP)
            {
                //如果是多级目录，需要递归创建
                //先创建年月文件夹
                string sRelativePath = _Config.FTPFullURI;
                foreach (string s in pathSegment)
                {
                    if (s.Trim() == string.Empty)
                    {
                        continue;
                    }
                    oFTPClient.MakeDir(s, sRelativePath);
                    sRelativePath += s + "/";
                }

                return true;
            }
            return false;
        }
    }
}
