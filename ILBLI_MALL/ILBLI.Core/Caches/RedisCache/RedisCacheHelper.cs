using ServiceStack.CacheAccess;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ILBLI.Core
{
    public class RedisCacheHelper:ICache
    {
        private RedisEnum RedisType { get; set; }
        private PooledRedisClientManager cache { get; set; }

        public RedisCacheHelper()
        {
            this.RedisType = RedisEnum.CommonRedis;
            cache = RedisConfig.CreateRedisConnection(RedisType);
        }

        public RedisCacheHelper(RedisEnum redisEnum)
        {
            this.RedisType = redisEnum;
            cache = RedisConfig.CreateRedisConnection(RedisType);
        }

        public string GetCache(string cacheKey)
        {
            try
            {
                using (ICacheClient client = cache.GetCacheClient())
                {
                   return  client.Get<string>(cacheKey);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Redis获取缓存失败{e.Message}");
            }
        }

        public T GetCache<T>(string cacheKey) where T : class
        {
            try
            {
                using (ICacheClient client = cache.GetCacheClient())
                {
                    return client.Get<T>(cacheKey);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Redis获取缓存失败{e.Message}");
            }
        }

        public void WriteCache<T>(T value, string cacheKey) where T : class
        {
            try
            {
                using (ICacheClient client = cache.GetCacheClient())
                {
                    client.Set<T>(cacheKey, value);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Redis写入缓存失败{e.Message}");
            }
        }

        public void WriteCache<T>(T value, string cacheKey, DateTime expireTime) where T : class
        {
            try
            {
                using (ICacheClient client = cache.GetCacheClient())
                {
                    client.Set<T>(cacheKey, value,expireTime);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Redis写入缓存失败{e.Message}");
            }
        }

        public void RemoveCache(string cacheKey)
        {
            try
            {
                using (ICacheClient client = cache.GetCacheClient())
                {
                    client.Remove(cacheKey);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Redis删除指定的缓存失败{e.Message}");
            }
        }

        public void RemoveCache(IEnumerable<string> keyList)
        {
            try
            {
                using (ICacheClient client = cache.GetCacheClient())
                {
                    client.RemoveAll(keyList);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Redis删除指定的缓存组失败{e.Message}");
            }
        }

        public bool ExsitCache(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public void ExtendCache(string cacheKey, TimeSpan span)
        {
            throw new NotImplementedException();
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }
    }
}
