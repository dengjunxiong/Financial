using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Collections;

namespace Financial.CommonLib.Mail
{
    /// <summary>
    /// 邮件配置器
    /// </summary>
    [Serializable]
    public class MailSettings
    {
        /// <summary>
        /// 配置集合
        /// </summary>
        public List<MailSenderInfo> Senders = new List<MailSenderInfo>();
        private Hashtable _hashByName = new Hashtable();//Hash集合

        private static MailSettings _current = null;
        /// <summary>
        /// 获得当前配置集合
        /// </summary>
        public static MailSettings Current
        {
            get
            {
                if (_current == null)
                {
                    _current = FromFile(FilePath);
                }
                return _current;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void initialHash()
        {
            _hashByName.Clear();

            foreach (MailSenderInfo oInfo in Senders)
            {
                _hashByName.Add(oInfo.Name, oInfo);
            }
        }

        /// <summary>
        /// 键值索引器
        /// </summary>
        /// <param name="sKey">键值</param>
        /// <returns>邮件配置</returns>
        public MailSenderInfo this[string sKey]
        {
            get
            {
                MailSenderInfo oRet = (MailSenderInfo)_hashByName[sKey];
                if (oRet == null)
                {
                    //这里可以初始化一个默认的发送账户，也可以抛出异常；
                    throw new Exception("发送账户没有配置");
                }
                return oRet;
            }
        }

        /// <summary>
        /// 文件保存的路径
        /// </summary>
        public static string FilePath
        {
            get
            {
                string path = Configuration.ConfigurationPath;
                return string.Format("{0}{1}", path, "mailsetting.xml");
            }
        }

        /// <summary>
        /// 强制重新加载
        /// </summary>
        public static void Invalidate()
        {
            _current = null;
        }

        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="sFileName">文件路径</param>
        public void SaveToFile(string sFileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(MailSettings));
            Stream stream = new FileStream(sFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            xs.Serialize(stream, this);
            stream.Close();
        }

        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="sFileName">文件路径</param>
        /// <returns>邮件配置器</returns>
        public static MailSettings FromFile(string sFileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(MailSettings));
            Stream stream = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            MailSettings oRet = (MailSettings)xs.Deserialize(stream);
            stream.Close();

            oRet.initialHash();
            return oRet;
        }
    }
}
