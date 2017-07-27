using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Financial.CommonLib
{
    /// <summary>
    /// 常量定义
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// 临时文件夹相对于根目录的路径(即Web项目根文件夹)
        /// </summary>
        public const string temp_folder = "\\TEMP\\";
        /// <summary>
        /// 转换程序相对于根目录的路径(即Web项目根文件夹的父文件夹)
        /// </summary>
        public const string pdf_to_swf_exePath = "\\lib\\PdfToSwf.exe";

        private static string tempFolder = null;
        /// <summary>
        /// 临时上传文件夹
        /// </summary>
        public static string TempFolder
        {
            get
            {
                if (tempFolder == null)
                {
                    string path = StringHelper.DelLastStr(System.AppDomain.CurrentDomain.BaseDirectory, "\\");
                    tempFolder = path + temp_folder;
                }
                return tempFolder;
            }
        }

        private static string pdfToSwfExePath = null;
        /// <summary>
        /// 转换程序路径
        /// </summary>
        public static string PdfToSwfExePath
        {
            get
            {
                if (pdfToSwfExePath == null)
                {
                    string path = StringHelper.DelLastStr(System.AppDomain.CurrentDomain.BaseDirectory, "\\");
                    path = path.Substring(0, path.LastIndexOf("\\"));
                    pdfToSwfExePath = path + pdf_to_swf_exePath;
                }
                return pdfToSwfExePath;
            }
        }

        #region SESSION
        /// <summary>
        /// 登录验证码SESSION键值
        /// </summary>
        public const string login_session_name = "login_verify_code";
        /// <summary>
        /// 注册验证码SESSION键值
        /// </summary>
        public const string register_session_name = "register_verify_code";

        #endregion SESSION
    }
}
