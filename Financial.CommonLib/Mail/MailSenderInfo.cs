using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Financial.CommonLib.Mail
{
    /// <summary>
    /// 邮件配置信息
    /// </summary>
    [Serializable]
    public class MailSenderInfo
    {
        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _DisplayName = string.Empty;
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }

        private string _address;
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private string _user;
        /// <summary>
        /// 帐号
        /// </summary>
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        private string _Password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private string _smtp;
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Smtp
        {
            get { return _smtp; }
            set { _smtp = value; }
        }

        private bool _ssl;
        /// <summary>
        /// 是否开启SSL验证
        /// </summary>
        public bool Ssl
        {
            get { return _ssl; }
            set { _ssl = value; }
        }

        private int _post;
        /// <summary>
        /// SMTP服务器端口号
        /// </summary>
        public int Post
        {
            get { return _post; }
            set { _post = value; }
        }
    }
}
