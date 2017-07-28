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
    public class MailInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// SMTP服务器地址
        /// </summary>
        public string Smtp { get; set; }

        /// <summary>
        /// SMTP服务器端口号
        /// </summary>
        public int Post { get; set; }

        /// <summary>
        /// 是否开启SSL验证
        /// </summary>
        public bool EnableSSL { get; set; }
    }
}