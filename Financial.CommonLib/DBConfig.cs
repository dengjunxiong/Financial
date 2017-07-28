using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Financial.CommonLib
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public class DBConfig
    {
        public List<DBConnSet> List = new List<DBConnSet>();//数据库连接集合

        private static DBConfig current = null;
        /// <summary>
        /// 数据库连接当前配置
        /// </summary>
        public static DBConfig Current
        {
            get
            {
                if (current == null)
                {
                    current = FromFile();
                }
                return current;
            }
        }

        /// <summary>
        /// 数据库连接索引器
        /// </summary>
        /// <param name="key">键值(小写)</param>
        /// <returns>连接字符串</returns>
        public string this[string key]
        {
            get
            {
                foreach (var item in List)
                {
                    if (item.Key.ToLower() == key)
                    {
                        return item.ConnString;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 从配置文件读取数据库连接
        /// </summary>
        /// <returns>数据库连接配置</returns>
        public static DBConfig FromFile()
        {
            string dirPath = Configuration.ConfigurationPath;
            string filePath = string.Format("{0}{1}", dirPath, "conn.config");

            XmlSerializer xs = new XmlSerializer(typeof(DBConfig));
            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DBConfig config = (DBConfig)xs.Deserialize(stream);
            stream.Close();
            return config;
        }

        /// <summary>
        /// 将配置信息保存到配置文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        public void SaveToFile()
        {
            string dirPath = Configuration.ConfigurationPath;
            string filePath = string.Format("{0}{1}", dirPath, "conn.config");

            XmlSerializer xs = new XmlSerializer(typeof(DBConfig));
            Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            xs.Serialize(stream, this);
            stream.Close();
        }
    }

    /// <summary>
    /// 数据库连接信息
    /// </summary>
    public class DBConnSet
    {
        /// <summary>
        /// 键值
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnString { get; set; }
    }
}
