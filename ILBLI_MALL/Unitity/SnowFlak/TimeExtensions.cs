using System; 
namespace SnowFlake
{
    public static class System
    {
        public static Func<long> currentTimeFunc = InternalCurrentTimeMillis;

        private static readonly DateTime Jan1st1970 = new DateTime
          (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 获取毫秒时间戳
        /// </summary>
        /// <returns></returns>
        public static long CurrentTimeMillis()
        {
            return currentTimeFunc();
        }

        private static long InternalCurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}
