using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

namespace Financial.CommonLib.Mail
{
    /// <summary>
    /// 邮件发送
    /// </summary>
    public class MailSender
    {
        /// <summary>
        /// 邮件
        /// </summary>
        private MailMessage mailMessage;

        /// <summary>
        /// SMTP
        /// </summary>
        private SmtpClient smtpClient;

        /// <summary> 
        /// 构造函数 
        /// </summary>
        /// <param name="info">发件人信息</param> 
        /// <param name="address">收件人地址</param> 
        /// <param name="body">邮件正文</param> 
        /// <param name="title">邮件的标题</param> 
        public MailSender(MailInfo info, string address, string body, string title)
        {
            try
            {
                mailMessage = new MailMessage();
                mailMessage.To.Add(address);
                mailMessage.From = new MailAddress(info.Address, info.DisplayName);
                mailMessage.Subject = title;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Priority = MailPriority.Normal;

                smtpClient = new SmtpClient();
                smtpClient.Credentials = new NetworkCredential(mailMessage.From.Address, info.Password);//设置发件人身份的票据 
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Host = info.Smtp;
                if (info.EnableSSL)
                {
                    smtpClient.EnableSsl = info.EnableSSL;
                }
                smtpClient.Port = info.Post;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="Path">附件路径(多个附件以","分隔)</param>
        private void Attachments(string Path)
        {
            string[] path = Path.Split(',');
            Attachment data;
            ContentDisposition disposition;
            for (int i = 0; i < path.Length; i++)
            {
                data = new Attachment(path[i], MediaTypeNames.Application.Octet);//实例化附件 
                disposition = data.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(path[i]);//获取附件的创建日期 
                disposition.ModificationDate = File.GetLastWriteTime(path[i]);//获取附件的修改日期 
                disposition.ReadDate = File.GetLastAccessTime(path[i]);//获取附件的读取日期 
                mailMessage.Attachments.Add(data);//添加到附件中 
            }
        }

        /// <summary> 
        /// 异步发送邮件 
        /// </summary> 
        /// <param name="completedMethod">发送完成处理</param> 
        /// <returns>结果(true:成功)</returns>
        private bool SendAsync(SendCompletedEventHandler completedMethod)
        {
            try
            {
                smtpClient.SendCompleted += new SendCompletedEventHandler(completedMethod);//注册异步发送邮件完成时的事件
                smtpClient.SendAsync(mailMessage, mailMessage.Body);
                return true;
            }
            catch
            {
            }
            return false;
        }

        /// <summary> 
        /// 发送邮件 
        /// </summary> 
        /// <returns>结果(true:成功)</returns>
        public bool Send()
        {
            try
            {
                smtpClient.Send(mailMessage);
                return true;
            }
            catch
            {
            }
            return false;
        }
    }
}
