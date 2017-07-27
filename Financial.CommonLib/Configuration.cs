using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Financial.CommonLib
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// 构造配置对象
        /// </summary>
        public Configuration()
        {

        }

        /// <summary>
        /// 日志文件夹路径
        /// </summary>
        public static string LogPath = ConfigurationManager.AppSettings["LogPath"];

        /// <summary>
        /// 配置文件夹路径
        /// </summary>
        public static string ConfigurationPath = ConfigurationManager.AppSettings["ConfigPath"];

        /// <summary>
        /// 图片域名
        /// </summary>
        public static string ImageRootPath = ConfigurationManager.AppSettings["ImagePath"];

        /// <summary>
        /// 图标域名
        /// </summary>
        public static string IconRootPath = ConfigurationManager.AppSettings["IconPath"];

        /// <summary>
        /// 空白图片地址
        /// </summary>
        public static string EmptyImagePath = ConfigurationManager.AppSettings["EmptyImagePath"];

        /// <summary>
        /// 获取图片地址
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns>图片地址</returns>
        public static string GetImgPath(string path)
        {
            if (path == null || path.Length == 0)
            {
                return EmptyImagePath;
            }
            if (path.IndexOf("/") == 0)//"/"符号在起始位置
            {
                return string.Format("{0}{1}", ImageRootPath, path);
            }
            return string.Format("{0}/{1}", ImageRootPath, path);
        }

        /// <summary>
        /// 获取图片地址
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns>图片地址</returns>
        public static string GetIconPath(string path)
        {
            if (path == null || path.Length == 0)
            {
                return EmptyImagePath;
            }
            if (path.IndexOf("/") == 0)//"/"符号在起始位置
            {
                return string.Format("{0}{1}", IconRootPath, path);
            }
            return string.Format("{0}/{1}", IconRootPath, path);
        }
    }
}