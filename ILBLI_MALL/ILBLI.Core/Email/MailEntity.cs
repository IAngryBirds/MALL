using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ILBLI.Core
{
    public class MailEntity
    {
        public MailEntity()
        {
            BodyEncoding = Encoding.Default;
            IsBodyHtml = true;
        }

        /// <summary>
        /// 发送人
        /// </summary>
        public string MailFrom { get; set; }

        /// <summary>
        /// 邮件主题
        /// </summary>
        public string MailSubject { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string MailBody { get; set; }

        /// <summary>
        /// 收件人列表
        /// </summary>
        public List<string> MailTo { get; set; }

        /// <summary>
        /// CC列表
        /// </summary>
        public List<string> MailCC { get; set; }

        /// <summary>
        /// 附件列表
        /// </summary>
        public List<Attachment> AttachmentList { get; set; }

        /// <summary>
        /// 邮件内容的编码格式
        /// </summary>
        public Encoding BodyEncoding { get; set; }

        /// <summary>
        /// 是否显示为HTML格式
        /// </summary>
        public bool IsBodyHtml { get; set; }
    }
}
