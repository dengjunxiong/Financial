using System;
using System.Collections.Generic;
using System.IO;

namespace Financial.CommonLib.FileSys
{
    /// <summary>
    /// 文件上传类
    /// 该类实现Web文件上传到UNC映射
    /// </summary>
    public class FileUploader
    {
        private string _LastErrorMsg = string.Empty;
        private UploadConfig _Config = null;
        private UploadConfig _DownloadConfig = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oConfig">上传文件配置</param>
        public FileUploader(UploadConfig oConfig)
        {
            _Config = oConfig;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oConfig">上传文件配置</param>
        /// <param name="dConfig">下载文件配置</param>
        public FileUploader(UploadConfig oConfig, UploadConfig dConfig)
        {
            _Config = oConfig;
            _DownloadConfig = dConfig;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="filename">文件名</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(string path, string filename)
        {
            try
            {
                if (File.Exists(path + filename))
                {
                    File.Delete(path + filename);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 直接将Web发送上来的文件流上传到服务器
        /// </summary>
        /// <param name="oInfo">上传提交文件</param>
        /// <param name="nFileID">文件ID</param>
        /// <returns>上传结果</returns>
        public UploadResult Upload(UploadPostFileInfo oInfo, int nFileID)
        {
            UploadResult oRet = new UploadResult();

            string sFilePath = string.Empty;
            string sFileName = string.Empty;

            //设置路径格式
            string path = string.Format("{0}{1}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"));
            string sPath = string.Format("{0}{1}\\", _Config.UploadPath, path);

            try
            {
                //创建目录
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }

                //生成文件名
                //文件名格式：ID + GUID
                string sFileTitle = string.Format("{0}_{1}_{2}", nFileID, DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString(), (new Random().Next(1, 1000000)).ToString());

                sFileName = sFileTitle + "." + oInfo.FilExt;

                Stream oFileStream = oInfo.PostedFile.InputStream;

                IList<UploadThumbResult> thumbList = null;
                //是否是图片类型文件
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
                                    thumbName = string.Format("{0}_thumb.{1}", sFileTitle, "png");
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
                byte[] buffer = new byte[oFileStream.Length];
                oFileStream.Read(buffer, 0, buffer.Length);

                System.IO.FileStream fs = new System.IO.FileStream(sPath + sFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
                fs.Close();

                oRet.ServerPath = _Config.UploadUrl + path + "/" + sFileName;
                if (thumbList != null && thumbList.Count > 0)
                {
                    foreach (UploadThumbResult item in thumbList)
                    {
                        if (item.Success)
                        {
                            buffer = new byte[item.ThumbStream.Length];
                            item.ThumbStream.Read(buffer, 0, buffer.Length);

                            fs = new System.IO.FileStream(sPath + item.Name, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Flush();
                            fs.Close();
                        }
                    }
                    oRet.ServerThumbPath = _Config.UploadUrl + path + "/" + thumbList[0].Name;
                }
                oRet.Success = true;
            }
            catch (Exception)
            {
                oRet.Success = false;
            }

            return oRet;
        }

        /// <summary>
        /// 直接将Web发送上来的文件流以及缩略图上传到服务器
        /// </summary>
        /// <param name="fileList">上传提交文件(第一个文件是源文件,其他是缩略图)</param>
        /// <param name="widthList">文件宽度(只有缩略图宽度,比文件少一个)</param>
        /// <param name="heightList">文件高度(只有缩略图高度,比文件少一个)</param>
        /// <param name="nFileID">文件ID</param>
        /// <returns>上传结果</returns>
        public UploadResult Upload(IList<UploadPostFileInfo> fileList, IList<int> widthList, IList<int> heightList, int nFileID)
        {
            UploadResult oRet = new UploadResult();

            string sFilePath = string.Empty;
            string sFileName = string.Empty;
            IList<UploadThumbResult> thumbList = null;

            //设置路径格式
            string path = string.Format("{0}{1}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"));
            string sPath = string.Format("{0}{1}\\", _Config.UploadPath, path);
            try
            {
                string fileExt = fileList[0].FilExt;
                //创建目录
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }

                //生成文件名
                //文件名格式：ID + GUID
                string sFileTitle = string.Format("{0}_{1}_{2}", nFileID, DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString(), (new Random().Next(1, 1000000)).ToString());

                sFileName = sFileTitle + "." + fileExt;

                Stream oFileStream = fileList[0].PostedFile.InputStream;

                //是否成功生成缩略图
                if (fileList[0].FilType == eFileType.Image)
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
                        bool isExists = false;
                        for (int i = 0; i < thumbWidth.Count; i++)
                        {
                            isExists = false;
                            foreach (int item in widthList)
                            {
                                if (thumbWidth[i] == item.ToString())
                                {
                                    isExists = true;
                                    break;
                                }
                            }
                            if (isExists)
                            {
                                continue;
                            }
                            if (thumbHeight.Count > i)//高度与宽度个数保持一致
                            {
                                if (i == 0)
                                {
                                    thumbName = string.Format("{0}_thumb.{1}", sFileTitle, "png");
                                }
                                else
                                {
                                    thumbName = string.Format("{0}_thumb_{1}_{2}.{3}", sFileTitle, thumbWidth[i], thumbHeight[i], "png");
                                }
                                thumbResult = new UploadThumbResult(thumbName, thumbWidth[i], thumbHeight[i]);
                                thumbResult.Success = oIH.GenThumbNail(fileList[0].FilExt, oFileStream, new System.Drawing.Size(thumbResult.Width, thumbResult.Height), thumbResult.ThumbStream);
                                fileList[0].PostedFile.InputStream.Seek(0, SeekOrigin.Begin);
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
                byte[] buffer = new byte[oFileStream.Length];
                oFileStream.Read(buffer, 0, buffer.Length);

                System.IO.FileStream fs = new System.IO.FileStream(sPath + sFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
                fs.Close();

                oRet.ServerPath = _Config.UploadUrl + path + "/" + sFileName;

                oRet.ServerThumbPath = _Config.UploadUrl + path + "/" + string.Format("{0}_thumb.{1}", sFileTitle, fileExt);
                if (fileList.Count > 1)
                {
                    fileList.RemoveAt(0);
                    string thumbName = "";
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                thumbName = string.Format("{0}_thumb.{1}", sFileTitle, "png");
                                oRet.ServerThumbPath = _Config.UploadUrl + path + "/" + thumbName;
                                break;
                            default:
                                thumbName = string.Format("{0}_thumb_{1}_{2}.{3}", sFileTitle, widthList[i], heightList[i], "png");
                                break;
                        }
                        oFileStream = fileList[i].PostedFile.InputStream;
                        buffer = new byte[oFileStream.Length];
                        oFileStream.Read(buffer, 0, buffer.Length);

                        fs = new System.IO.FileStream(sPath + thumbName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Flush();
                        fs.Close();
                    }
                }
                if (thumbList != null && thumbList.Count > 0)
                {
                    foreach (UploadThumbResult item in thumbList)
                    {
                        if (item.Success)
                        {
                            buffer = new byte[item.ThumbStream.Length];
                            item.ThumbStream.Read(buffer, 0, buffer.Length);

                            fs = new System.IO.FileStream(sPath + item.Name, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Flush();
                            fs.Close();
                        }
                    }
                    oRet.ServerThumbPath = _Config.UploadUrl + path + "/" + thumbList[0].Name;
                }
                oRet.Success = true;
            }
            catch (Exception)
            {
                oRet.Success = false;
            }

            return oRet;
        }

        /// <summary>
        /// 直接将文件流上传到服务器
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

            //设置路径格式
            string path = string.Format("{0}{1}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"));
            string sPath = string.Format("{0}{1}\\", _Config.UploadPath, path);
            try
            {
                //创建目录
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }


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

                    //上传文件流到服务器
                    byte[] buffer = new byte[oFileStream.Length];
                    oFileStream.Read(buffer, 0, buffer.Length);

                    System.IO.FileStream fs = new System.IO.FileStream(sPath + sFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Flush();
                    fs.Close();

                    oRet.ServerPath = _Config.UploadUrl + path + "/" + sFileName;

                    if (thumbList != null && thumbList.Count > 0)
                    {
                        foreach (UploadThumbResult item in thumbList)
                        {
                            if (item.Success)
                            {
                                //上传缩略图流到文件服务器
                                buffer = new byte[item.ThumbStream.Length];
                                item.ThumbStream.Read(buffer, 0, buffer.Length);

                                fs = new System.IO.FileStream(sPath + item.Name, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                                fs.Write(buffer, 0, buffer.Length);
                                fs.Flush();
                                fs.Close();
                            }
                        }
                        oRet.ServerThumbPath = _Config.UploadUrl + path + "/" + thumbList[0].Name;
                    }
                }
                oRet.Success = true;
            }
            catch (Exception)
            {
                oRet.Success = false;
            }

            return oRet;
        }

        /// <summary>
        /// 直接将Web发送上来的文件流上传到服务器
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
            //设置路径格式
            string path = string.Format("{0}{1}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"));
            string sPath = string.Format("{0}{1}\\", _Config.UploadPath, path);

            sFileName = sFileTitle + "." + "swf";

            FileInfo fileInf = null;
            FileStream fs = null;
            try
            {
                //创建目录
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }
                fileInf = new FileInfo(string.Format("{0}{1}", tempFolder, sFileName));
                fs = fileInf.OpenRead();

                sPath = sPath.Replace('\\', '/');

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);

                System.IO.FileStream fsW = new System.IO.FileStream(sPath + sFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                fsW.Write(buffer, 0, buffer.Length);
                fsW.Flush();
                fsW.Close();
                oRet.Success = true;
                oRet.ServerPath = _Config.UploadUrl + path + "/" + sFileName;
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
    }
}