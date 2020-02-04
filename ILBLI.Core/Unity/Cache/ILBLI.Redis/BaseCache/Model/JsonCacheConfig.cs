using System;
using System.Collections.Generic;

namespace ILBLI.Redis
{
    public  class JsonCacheConfig
    {

        public List<CacheModel> JsonCaches { get; set; }

        /// <summary>
        /// 获取缓存信息
        /// </summary>
        /// <param name="cacheKey">缓存Key</param> 
        /// <returns></returns>
        public CacheModel GetCacheModel(CacheKey cacheKey)
        {
            var queryModel = JsonCaches.Find(x => x.CacheType.Equals(cacheKey.ToString(),StringComparison.OrdinalIgnoreCase));
            return queryModel.Clone() as CacheModel;
        }
    }
}
