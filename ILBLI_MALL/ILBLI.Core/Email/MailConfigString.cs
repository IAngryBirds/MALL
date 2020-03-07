namespace ILBLI.Core
{
    /// <summary>
    /// 运行时编译
    /// </summary>
    public class MailConfigString
    {
        /// <summary>
        /// 邮箱服务器地址
        /// </summary>
       public static readonly string SMTPServer= ConfigManager.GetConfigAppsettingValueByKey("SMTPServer") ;
        /// <summary>
        /// 邮件端口
        /// </summary>
       public static readonly string ServerPort = ConfigManager.GetConfigAppsettingValueByKey("SMTPServerPort");
        /// <summary>
        /// 发送邮件得账户
        /// </summary>
       public static readonly string EmailAccount = ConfigManager.GetConfigAppsettingValueByKey("EmailAccount");
        /// <summary>
        /// 发送邮件得账户密码
        /// </summary>
       public static readonly string EmailPassword = ConfigManager.GetConfigAppsettingValueByKey("EMailPassword");
    }
}
