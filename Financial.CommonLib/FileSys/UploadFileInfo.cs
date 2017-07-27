using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace Financial.CommonLib.FileSys
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public enum eFileType
    {
        /// <summary>
        /// 未知文件
        /// </summary>
        Unkown = 0,
        /// <summary>
        /// 图片文件
        /// </summary>
        Image = 1,
        /// <summary>
        /// 文档文件
        /// </summary>
        Doc = 2,
        /// <summary>
        /// 视频文件
        /// </summary>
        Vedio = 3,
        /// <summary>
        /// Flash文件
        /// </summary>
        Flash = 4,
        /// <summary>
        /// Office文件
        /// </summary>
        Office = 5,
        /// <summary>
        /// 微课网支持视频文件
        /// </summary>
        WeiKeVideo = 6,
        /// <summary>
        /// 微课网支持文档文件
        /// </summary>
        WeiKeDoc = 7
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    public class UploadFileInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UploadFileInfo()
        {

        }

        /// <summary>
        /// 解析文件
        /// </summary>
        virtual public void Parse()
        {

        }

        #region 属性

        private string _FilExt;
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FilExt
        {
            get { return _FilExt; }
            set { _FilExt = value; }
        }

        private string _FilOrignalName;
        /// <summary>
        /// 原文件文件名
        /// </summary>
        public string FilOrignalName
        {
            get { return _FilOrignalName; }
            set { _FilOrignalName = value; }
        }

        private eFileType _FilType;
        /// <summary>
        /// 文件类型
        /// </summary>
        public eFileType FilType
        {
            get { return _FilType; }
            set { _FilType = value; }
        }

        private long _FilSize;
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FilSize
        {
            get { return _FilSize; }
            set { _FilSize = value; }
        }

        #endregion 属性

        #region 方法

        /// <summary>
        /// 获取文件名称（包含扩展名）
        /// </summary>
        /// <param name="sFileName">文件名称</param>
        /// <returns>文件名称</returns>
        public static string GetFileTitle(string sFileName)
        {
            if (sFileName.Trim().Length == 0)
            {
                return string.Empty;
            }
            int nPos = sFileName.LastIndexOf("\\");
            if (nPos == -1)
            {
                return sFileName;
            }
            return sFileName.Substring(nPos + 1);
        }

        /// <summary>
        /// 获取文件扩展名(小写)
        /// </summary>
        /// <param name="sFileName">文件名称</param>
        /// <returns>文件扩展名</returns>
        public static string GetFileExtension(string sFileName)
        {
            int nPos = sFileName.LastIndexOf(".");
            if (nPos == -1)
            {
                return string.Empty;
            }
            return sFileName.Substring(nPos + 1).ToLower();
        }

        /// <summary>
        /// 根据文件扩展名确定文件类型
        /// </summary>
        /// <param name="sExt">文件扩展名</param>
        /// <returns>文件类型</returns>
        public static eFileType GetFileType(string sExt)
        {
            if (IsValidImage(sExt))
            {
                return eFileType.Image;
            }
            if (IsValidDoc(sExt))
            {
                return eFileType.Doc;
            }
            if (IsValidVedio(sExt))
            {
                return eFileType.Vedio;
            }
            if (IsValidFlash(sExt))
            {
                return eFileType.Flash;
            }
            return eFileType.Unkown;
        }

        /// <summary>
        /// 确定当前文件的文件类型
        /// </summary>
        /// <returns>文件类型</returns>
        public eFileType ParseFileType()
        {
            if (IsWeiKeVideo(this.FilExt))
            {
                return eFileType.WeiKeVideo;
            }
            if (IsWeiKeDoc(this.FilExt))
            {
                return eFileType.WeiKeDoc;
            }
            if (IsVilidOffice(this.FilExt))
            {
                return eFileType.Office;
            }
            if (IsValidVedio(this.FilExt))
            {
                return eFileType.Vedio;
            }
            if (IsValidFlash(this.FilExt))
            {
                return eFileType.Flash;
            }
            if (IsValidImage(this.FilExt))
            {
                return eFileType.Image;
            }
            if (IsValidDoc(this.FilExt))
            {
                return eFileType.Doc;
            }
            return eFileType.Unkown;
        }

        /// <summary>
        /// 是否是合法的图片文件
        /// </summary>
        /// <param name="sFilExt">文件扩展名(不包含".")</param>
        /// <returns>是否是图片文件</returns>
        public static bool IsValidImage(string sFilExt)
        {
            string sFilter = "jpg,jpeg,gif,png,bmp,ico";
            string[] s = sFilter.Split(',');
            for (int i = 0; i < s.Length; i++)
            {
                if (sFilExt == s[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否是合法的文档文件
        /// </summary>
        /// <param name="sFilExt">文件扩展名(不包含".")</param>
        /// <returns>是否是文档文件</returns>
        public static bool IsValidDoc(string sFilExt)
        {
            string sFilter = "doc,rar,zip,ppt,xls,pdf";
            string[] s = sFilter.Split(',');
            for (int i = 0; i < s.Length; i++)
            {
                if (sFilExt == s[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否是合法的Office文件
        /// </summary>
        /// <param name="sFilExt">文件扩展名(不包含".")</param>
        /// <returns>是否是Office文件</returns>
        public static bool IsVilidOffice(string sFilExt)
        {
            string sFilter = "doc,docx,ppt,pptx,xls,xlsx,pdf";
            string[] s = sFilter.Split(',');
            for (int i = 0; i < s.Length; i++)
            {
                if (sFilExt == s[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否是合法的视频文件
        /// </summary>
        /// <param name="sFilExt">文件扩展名(不包含".")</param>
        /// <returns>是否是视频文件</returns>
        public static bool IsValidVedio(string sFilExt)
        {
            string sFilter = "mpg,mpeg,asf,avi,rm,wmv,flv";
            string[] s = sFilter.Split(',');
            for (int i = 0; i < s.Length; i++)
            {
                if (sFilExt == s[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否是合法的Flash文件
        /// </summary>
        /// <param name="sFilExt">文件扩展名(不包含".")</param>
        /// <returns>是否是Flash文件</returns>
        public static bool IsValidFlash(string sFilExt)
        {
            string sFilter = "swf,flv";
            string[] s = sFilter.Split(',');
            for (int i = 0; i < s.Length; i++)
            {
                if (sFilExt == s[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否是合法的微课网视频文件
        /// </summary>
        /// <param name="sFilExt">文件扩展名(不包含".")</param>
        /// <returns>是否是微课网视频文件</returns>
        public static bool IsWeiKeVideo(string sFilExt)
        {
            string sFilter = "mp4,flv,wmv,rm,rmvb";
            string[] s = sFilter.Split(',');
            for (int i = 0; i < s.Length; i++)
            {
                if (sFilExt == s[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否是合法的微课网文档文件
        /// </summary>
        /// <param name="sFilExt">文件扩展名(不包含".")</param>
        /// <returns>是否是微课网文档文件</returns>
        public static bool IsWeiKeDoc(string sFilExt)
        {
            string sFilter = "doc,docx,ppt,pptx,xls,xlsx,pdf,rar,zip";
            string[] s = sFilter.Split(',');
            for (int i = 0; i < s.Length; i++)
            {
                if (sFilExt == s[i])
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }

    /// <summary>
    /// 上传本地文件
    /// </summary>
    public class UploadLocalFileInfo : UploadFileInfo
    {

        #region 属性

        private FileInfo _LocalFile;
        /// <summary>
        /// 文件对象
        /// </summary>
        public FileInfo LocalFile
        {
            get { return _LocalFile; }
        }

        #endregion 属性

        /// <summary>
        /// 构造函数
        /// </summary>
        public UploadLocalFileInfo()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oFile">文件对象</param>
        public UploadLocalFileInfo(FileInfo oFile)
        {
            _LocalFile = oFile;
            Parse();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sFilename">文件名</param>
        public UploadLocalFileInfo(string sFilename)
        {
            _LocalFile = new FileInfo(sFilename);

            Parse();
        }

        /// <summary>
        /// 解析文件信息
        /// </summary>
        public override void Parse()
        {
            FilOrignalName = LocalFile.Name;
            FilExt = LocalFile.Extension;
            if (FilExt.Length > 0)
            {
                FilExt = FilExt.Substring(1); //消除“.”
            }
            FilSize = LocalFile.Length;
            FilType = ParseFileType();

            if (FilOrignalName.Length > 30)
            {
                FilOrignalName = FilOrignalName.Substring(0, 25) + LocalFile.Extension;
            }
        }
    }

    /// <summary>
    /// 上传提交文件
    /// </summary>
    public class UploadPostFileInfo : UploadFileInfo
    {
        #region 属性

        private HttpPostedFileBase _PostedFile;
        /// <summary>
        /// 提交的文件对象
        /// </summary>
        public HttpPostedFileBase PostedFile
        {
            get { return _PostedFile; }
        }

        #endregion 属性

        /// <summary>
        /// 构造函数
        /// </summary>
        public UploadPostFileInfo()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oFile">提交的文件对象</param>
        public UploadPostFileInfo(HttpPostedFile oFile)
        {
            _PostedFile = new HttpPostedFileWrapper(oFile);
            Parse();
        }

        /// <summary>
        /// 构造函数(MVC)
        /// </summary>
        /// <param name="oFile">提交的文件对象</param>
        public UploadPostFileInfo(HttpPostedFileBase oFile)
        {
            _PostedFile = oFile;
            Parse();
        }

        /// <summary>
        /// 解析提交文件
        /// </summary>
        override public void Parse()
        {
            FilOrignalName = GetFileTitle(_PostedFile.FileName);
            FilExt = GetFileExtension(_PostedFile.FileName);
            FilSize = _PostedFile.ContentLength;
            FilType = this.ParseFileType();
        }
    }

    /// <summary>
    /// 上传结果
    /// </summary>
    public class UploadResult
    {
        private bool _Success = false;
        /// <summary>
        /// 是否上传成功
        /// </summary>
        public bool Success
        {
            get { return _Success; }
            set { _Success = value; }
        }

        private string _Message = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        private string _ServerPath = string.Empty;
        /// <summary>
        /// 上传后的路径
        /// </summary>
        public string ServerPath
        {
            get { return _ServerPath; }
            set { _ServerPath = value; }
        }

        private string _ServerThumbPath = string.Empty;
        /// <summary>
        /// 缩略图路径
        /// </summary>
        public string ServerThumbPath
        {
            get { return _ServerThumbPath; }
            set { _ServerThumbPath = value; }
        }
    }

    /// <summary>
    /// 上传缩略图结果
    /// </summary>
    public class UploadThumbResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">缩略图文件名</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        public UploadThumbResult(string name, string width, string height)
        {
            this.Name = name;
            this.ThumbStream = new MemoryStream();
            this.Width = ConvertHelper.StrToInt(width, 80);
            this.Height = ConvertHelper.StrToInt(height, 80);
        }

        private bool success = false;
        /// <summary>
        /// 是否成功生成缩略图
        /// </summary>
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        private MemoryStream thumbStream = null;
        /// <summary>
        /// 缩略图文件流
        /// </summary>
        public MemoryStream ThumbStream
        {
            get { return thumbStream; }
            set { thumbStream = value; }
        }

        private string name = null;
        /// <summary>
        /// 缩略图文件名
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int width = 80;
        /// <summary>
        /// 缩略图宽度
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int height = 80;
        /// <summary>
        /// 缩略图高度
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

    }
}
