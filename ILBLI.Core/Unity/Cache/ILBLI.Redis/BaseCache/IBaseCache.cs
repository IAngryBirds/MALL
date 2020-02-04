using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Threading.Tasks;

namespace ILBLI.Redis
{
    public interface IBaseCache
    {
        /// <summary>
        /// 添加或更改Redis的键值，并设置缓存的过期策略
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="distributedCacheEntryOptions">设置Redis缓存的过期策略，可以用其设置缓存的绝对过期时间（AbsoluteExpiration或AbsoluteExpirationRelativeToNow），也可以设置缓存的滑动过期时间（SlidingExpiration）</param>
        void Set(string key, object value, DistributedCacheEntryOptions distributedCacheEntryOptions);

        /// <summary>
        /// 查询键值是否在Redis中存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>true：存在，false：不存在</returns>
        bool Exist(string key);

        /// <summary>
        /// 从Redis中获取键值
        /// </summary>
        /// <typeparam name="T">缓存的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="isExisted">是否获取到键值，true：获取到了，false：键值不存在</param>
        /// <returns>缓存的对象</returns>
        T Get<T>(string key, out bool isExisted);

        /// <summary>
        /// 从Redis中删除键值，如果键值在Redis中不存在，该方法不会报错，只是什么都不会发生
        /// </summary>
        /// <param name="key">缓存键</param>
        void Remove(string key);

        /// <summary>
        /// 从Redis中刷新键值
        /// </summary>
        /// <param name="key">缓存键</param>
        void Refresh(string key);
        
        #region 自定义缓存

        /*
        /// <summary>
        /// 获取Key缓存键
        /// </summary> 
        /// <param name="cacheKey">获取缓存Key</param>
        /// <returns></returns>
        CacheModel GetCustomKey(CacheKey cacheKey);

        /// <summary>
        /// 获取缓存Key
        /// </summary>
        /// <param name="cacheKey">缓存类型</param>
        /// <param name="replaceStr">key的替换目标（原）</param>
        /// <param name="replaceKey">key的替换部分（新）</param>
        /// <returns></returns>
        CacheModel GetCustomKey(CacheKey cacheKey, string replaceStr, string replaceKey);
       
        /// <summary>
        /// 缓存自定义规则的缓存模式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">缓存类型</param>
        /// <param name="model">需要缓存的值</param>
        /// <returns></returns>
        bool CacheCustomSetValue<T>(CacheKey cacheKey, T model) where T : class;

        /// <summary>
        /// 缓存自定义规则的获取模式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">缓存类型</param>     
        /// <param name="keyID">key的替换部分</param>
        /// <returns></returns>
        T CacheCustomGetValue<T>(CacheKey cacheKey, string keyID) where T : class;

        /// <summary>
        /// 缓存自定义规则的缓存模式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">缓存类型</param>
        /// <param name="model">需要缓存的值</param>
        /// <param name="replaceStr">key的替换目标（原）</param>
        /// <param name="replaceKey">key的替换部分（新）</param>
        /// <returns></returns>
        bool CacheCustomSetValue<T>(CacheKey cacheKey, T model, string replaceStr, string replaceKey) where T : class;

        /// <summary>
        /// 缓存自定义规则的缓存模式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">缓存类型</param> 
        /// <returns></returns>
        T CacheCustomGetValue<T>(CacheKey cacheKey) where T : class;

        /// <summary>
        /// 缓存自定义规则的删除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        bool CacheCustomRemoveValue(CacheKey cacheKey);

        /// <summary>
        /// 缓存自定义规则的删除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="replaceStr">key的替换目标（原）</param>
        /// <param name="replaceKey">key的替换部分（新）</param>
        /// <returns></returns>
        bool CacheCustomRemoveValue(CacheKey cacheKey,string replaceStr, string replaceKey);
*/
        #endregion

    }
}
