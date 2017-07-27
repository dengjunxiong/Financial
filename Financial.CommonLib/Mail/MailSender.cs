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
    /// 邮件发布
    /// </summary>
    public class MailSender
    {
        private MailMessage mailMessage;
        private SmtpClient smtpClient;
        /// <summary>
        /// 发件人密码
        /// </summary>
        private string password;
        /// <summary>
        /// 邮件服务地址
        /// </summary>
        private string smtp;
        /// <summary>
        /// 是否开启SSL验证
        /// </summary>
        private bool enableSsl = false;
        /// <summary>
        /// SMTP服务器端口号
        /// </summary>
        private int post = 25;

        /// <summary> 
        /// 处审核后类的实例 
        /// </summary>
        /// <param name="mailModel">发件人地址</param> 
        /// <param name="To">收件人地址</param> 
        /// <param name="Body">邮件正文</param> 
        /// <param name="Title">邮件的主题</param> 
        public MailSender(MailSenderInfo mailModel, string To, string Body, string Title)
        {
            try
            {
                mailMessage = new MailMessage();
                mailMessage.To.Add(To);
                mailMessage.From = new MailAddress(mailModel.Address, mailModel.DisplayName);
                mailMessage.Subject = Title;
                mailMessage.Body = Body;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Priority = MailPriority.Normal;
                this.password = mailModel.Password;
                this.smtp = mailModel.Smtp;
                this.enableSsl = mailModel.Ssl;
                this.post = mailModel.Post;
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
        /// <param name="CompletedMethod"></param> 
        private void SendAsync(SendCompletedEventHandler CompletedMethod)
        {
            if (mailMessage != null)
            {
                smtpClient = new SmtpClient();
                smtpClient.Credentials = new NetworkCredential(mailMessage.From.Address, password);//设置发件人身份的票据 
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Host = "smtp." + mailMessage.From.Host;
                smtpClient.SendCompleted += new SendCompletedEventHandler(CompletedMethod);//注册异步发送邮件完成时的事件
                if (enableSsl)
                {
                    smtpClient.EnableSsl = enableSsl;
                }
                smtpClient.Port = post;
                smtpClient.SendAsync(mailMessage, mailMessage.Body);
            }
        }

        /// <summary> 
        /// 发送邮件 
        /// </summary> 
        public bool Send()
        {
            try
            {
                if (mailMessage != null)
                {
                    smtpClient = new SmtpClient();
                    smtpClient.Credentials = new NetworkCredential(mailMessage.From.Address, password);//设置发件人身份的票据 
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Host = smtp;
                    if (enableSsl)
                    {
                        smtpClient.EnableSsl = enableSsl;
                    }
                    smtpClient.Port = post;
                    smtpClient.Send(mailMessage);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
