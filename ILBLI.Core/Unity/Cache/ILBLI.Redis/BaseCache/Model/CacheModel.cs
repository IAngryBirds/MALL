using System;

namespace ILBLI.Redis
{
    /// <summary>
    /// 缓存实体类
    /// </summary>
    public class CacheModel : ICloneable
    {
        /// <summary>
        /// 获取缓存时间（以分钟为单位）
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetCacheTime()
        {
            return new TimeSpan(CacheTime * 600000000);
        }

        /// <summary>
        /// 缓存时间（分钟）
        /// </summary>
        public long CacheTime { private get; set; }

        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheType { get; set; }

        /// <summary>
        /// 缓存字符串
        /// </summary>
        public string CacheStr { get; set; }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
