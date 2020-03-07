namespace ILBLI.Core
{
    public class RabbitConfigString
    {
        /// <summary>
        /// 连接服务器地址//MQConnectionSetting=Host IP|;userid;|;password
        /// </summary>
        public static readonly string MQConnectionSetting = ConfigManager.GetConfigAppsettingValueByKey("MQConnectionSetting");

        /// <summary>
        /// 最大连接数
        /// </summary>
        public static readonly string MQMaxConnectionCount = ConfigManager.GetConfigAppsettingValueByKey("MQMaxConnectionCount");
    }
}
