using System;
using System.Net;
using System.Net.Mail;

namespace ILBLI.Core
{
    public class MailManager
    {
        private static string _SMTPServer;
        private static string _serverPort;
        private static string _userAccount;
        private static string _password;
        public MailManager()
        {
            // 初始化SMTP服务器信息
            _SMTPServer = MailConfigString.SMTPServer;
            _serverPort = MailConfigString.ServerPort;
            _userAccount = MailConfigString.EmailAccount;
            _password = MailConfigString.EmailPassword;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailEntity">邮件内容信息</param>
        /// <returns>是否发送成功</returns>
        public bool SendEmail(MailEntity mailEntity)
        {
            string resultMessage = string.Empty;
            return SendEmail(mailEntity, out resultMessage);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailEntity">邮件内容信息</param>
        /// <param name="resultMessage">发送结果信息</param>
        /// <returns>是否发送成功</returns>
        public bool SendEmail(MailEntity mailEntity, out string resultMessage)
        {
            if ((mailEntity.MailTo == null
                || mailEntity.MailTo.Count == 0)
                && (mailEntity.MailCC == null
                || mailEntity.MailCC.Count == 0))
            {
                // 记录发送失败日志
                resultMessage = string.Format("发送失败！，失败原因：需要至少设置一个收件人或抄送人。失败邮件标题：{0}", mailEntity.MailSubject);
                LogHelper.Warn(resultMessage);

                // 此类邮件不需要再次发送，故标记为已发送
                return true;
            }

            MailMessage message = new MailMessage();

            message.Subject = mailEntity.MailSubject;
            message.Body = mailEntity.MailBody;
            message.IsBodyHtml = mailEntity.IsBodyHtml;
            message.BodyEncoding = mailEntity.BodyEncoding;

            if (mailEntity.AttachmentList != null && mailEntity.AttachmentList.Count > 0)
            {
                foreach (var attachment in mailEntity.AttachmentList)
                {
                    message.Attachments.Add(attachment);
                }
            }

            string mailFrom = mailEntity.MailFrom;
            //如果没有设置发件人，使用默认发件人
            if (string.IsNullOrWhiteSpace(mailFrom))
            {
                mailFrom = _userAccount;
            }
            message.From = new MailAddress(mailFrom);
            message.To.Add(string.Join(",", mailEntity.MailTo.ToArray()));
            if (mailEntity.MailCC != null && mailEntity.MailCC.Count > 0)
            {
                message.CC.Add(string.Join(",", mailEntity.MailCC.ToArray()));
            }

            SmtpClient client = new SmtpClient(_SMTPServer);
            if (!string.IsNullOrWhiteSpace(_serverPort))
            {
                client.Port = Convert.ToInt32(_serverPort);
            }

            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(_userAccount, _password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                client.Send(message);
                System.Threading.Thread.Sleep(3000);
                resultMessage = "发送成功！";
                return true;
            }
            catch (Exception e)
            {
                // 记录异常日志
                resultMessage = string.Format("发送失败！，失败原因：{0}，堆栈信息：{1}", e.Message, e.StackTrace);
                LogHelper.Warn(e.Message, e);
                return false;
            }
        }
    }
}
