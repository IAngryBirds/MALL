using System;

namespace ILBLI.SnowFlak
{
    /// <summary>
    /// 系统扩展方法
    /// </summary>
    public static class System
    { 
        private static readonly DateTime mJan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 获取毫秒时间戳
        /// </summary>
        /// <returns></returns>
        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - mJan1st1970).TotalMilliseconds;
        } 
    }
}
