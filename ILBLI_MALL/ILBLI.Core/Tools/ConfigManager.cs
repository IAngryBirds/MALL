using System;
using System.Configuration;

namespace ILBLI.Core
{
    /// <summary>
    /// webConfig配置文件操作相关类
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// 获取配置文件的配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigAppsettingValueByKey(string key)
        {
            try
            {
                string strValue = ConfigurationManager.AppSettings.Get(key);
                return string.IsNullOrWhiteSpace(strValue) ? "" : strValue;
            }
            catch
            {
                throw new Exception($"读取_{key}_的配置信息失败");
            }
        }
    }
}
