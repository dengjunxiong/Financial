using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Financial.CommonLib.FileSys
{
    /// <summary>
    /// 文件服务器类型
    /// </summary>
    public enum FileServerTypes
    {
        /// <summary>
        /// 文件服务器(使用相对路径或绝对路径)
        /// </summary>
        UNC = 0,
        /// <summary>
        /// FTP服务器
        /// </summary>
        FTP = 1
    }

    /// <summary>
    /// 水印类型
    /// </summary>
    public enum WaterMarkTypes
    {
        /// <summary>
        /// 没有水印
        /// </summary>
        No = 1,
        /// <summary>
        /// 图片水印
        /// </summary>
        Picture = 2,
        /// <summary>
        /// 文本水印
        /// </summary>
        Text
    }

    /// <summary>
    /// 文件上传配置
    /// </summary>
    [Serializable]
    public class UploadConfig
    {
        /// <summary>
        /// 键值
        /// </summary>
        public string Key = string.Empty;

        /// <summary>
        /// 文件服务器类型
        /// </summary>
        public FileServerTypes ServerType = FileServerTypes.FTP;

        /// <summary>
        /// 上传路径
        /// </summary>
        public string UploadPath = string.Empty;

        /// <summary>
        /// 上传后用于显示的根URL
        /// </summary>
        public string UploadUrl = string.Empty;

        /// <summary>
        /// 是否生成缩略图
        /// </summary>
        public bool IsGenThumb = false;

        /// <summary>
        /// 缩略图宽度(多个宽度以","分隔)
        /// </summary>
        public string ThumbWidth = "80";

        /// <summary>
        /// 缩略图高度(多个高度以","分隔,高度与宽度个数保持一致)
        /// </summary>
        public string ThumbHeight = "80";

        /// <summary>
        /// 最大尺寸
        /// </summary>
        public int nMaxSize = 200;

        /// <summary>
        /// FTP服务器地址,如127.0.0.1，或者unionong.com
        /// </summary>
        public string FTPServerName = "localhost";

        /// <summary>
        /// FTP服务器端口，如“21”
        /// </summary>
        public string FTPServerPort = "21";

        /// <summary>
        /// 上传文件夹相对于FTP服务器的根路径
        /// </summary>
        public string FTPRootPath = "/";

        /// <summary>
        /// FTP用户名
        /// </summary>
        public string FTPUserName = string.Empty;

        /// <summary>
        /// FTP密码
        /// </summary>
        public string FTPPwd = string.Empty;

        /// <summary>
        /// 水印类型
        /// </summary>
        public WaterMarkTypes WaterMarktype = WaterMarkTypes.No;

        /// <summary>
        /// 水印图片路径
        /// </summary>
        public string WarterMarkPicPath = string.Empty;

        /// <summary>
        /// 水印文字
        /// </summary>
        public string WarterMarkText = string.Empty;

        /// <summary>
        /// 文字水印字体名称
        /// </summary>
        public string WarterMarkFontName = string.Empty;

        /// <summary>
        /// 文字水印字体大小
        /// </summary>
        public int WarterMarkFontSize = 12;

        /// <summary>
        /// 该属性自动计算FTP的全部URI，格式:ftp://FTPServerName:FTPServerPort + FTPRootPath
        /// </summary>
        public string FTPFullURI
        {
            get
            {
                return "ftp://" + FTPServerName + ":" + FTPServerPort.ToString() + FTPRootPath;
            }
        }
    }
}