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
        /// 配置文件夹路径
        /// </summary>
        public static string ConfigurationPath = ConfigurationManager.AppSettings["ConfigPath"];
    }
}