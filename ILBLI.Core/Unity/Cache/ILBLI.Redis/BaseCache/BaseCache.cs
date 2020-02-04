using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace ILBLI.Redis
{
    public class BaseCache : IBaseCache
    {
        private IDistributedCache _Cache;
        private JsonCacheConfig _JsonCacheConfig;

        /// <summary>
        /// 通过IDistributedCache来构造RedisCache缓存操作类
        /// </summary>
        /// <param name="cache">IDistributedCache对象</param>
        public BaseCache(IDistributedCache cache,IOptions<JsonCacheConfig> jsonCache)
        {
            this._Cache = cache;
            this._JsonCacheConfig = jsonCache.Value;
        }

        /// <summary>
        /// 添加或更改Redis的键值，并设置缓存的过期策略
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="distributedCacheEntryOptions">设置Redis缓存的过期策略，可以用其设置缓存的绝对过期时间（AbsoluteExpiration或AbsoluteExpirationRelativeToNow），也可以设置缓存的滑动过期时间（SlidingExpiration）</param>
        public void Set(string key, object value, DistributedCacheEntryOptions distributedCacheEntryOptions)
        {
            //通过Json.NET序列化缓存对象为Json字符串
            //调用JsonConvert.SerializeObject方法时，设置ReferenceLoopHandling属性为ReferenceLoopHandling.Ignore，来避免Json.NET序列化对象时，因为对象的循环引用而抛出异常
            //设置TypeNameHandling属性为TypeNameHandling.All，这样Json.NET序列化对象后的Json字符串中，会包含序列化的类型，这样可以保证Json.NET在反序列化对象时，去读取Json字符串中的序列化类型，从而得到和序列化时相同的对象类型
            var stringObject = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All
            });

            var bytesObject = Encoding.UTF8.GetBytes(stringObject);//将Json字符串通过UTF-8编码，序列化为字节数组

            _Cache.Set(key, bytesObject, distributedCacheEntryOptions);//将字节数组存入Redis
            Refresh(key);//刷新Redis
        }

        /// <summary>
        /// 查询键值是否在Redis中存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>true：存在，false：不存在</returns>
        public bool Exist(string key)
        {
            var bytesObject = _Cache.Get(key);//从Redis中获取键值key的字节数组，如果没获取到，那么会返回null

            if (bytesObject == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 从Redis中获取键值
        /// </summary>
        /// <typeparam name="T">缓存的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="isExisted">是否获取到键值，true：获取到了，false：键值不存在</param>
        /// <returns>缓存的对象</returns>
        public T Get<T>(string key, out bool isExisted)
        {
            var bytesObject = _Cache.Get(key);//从Redis中获取键值key的字节数组，如果没获取到，那么会返回null

            if (bytesObject == null)
            {
                isExisted = false;
                return default(T);
            }

            var stringObject = Encoding.UTF8.GetString(bytesObject);//通过UTF-8编码，将字节数组反序列化为Json字符串

            isExisted = true;

            //通过Json.NET反序列化Json字符串为对象
            //调用JsonConvert.DeserializeObject方法时，也设置TypeNameHandling属性为TypeNameHandling.All，这样可以保证Json.NET在反序列化对象时，去读取Json字符串中的序列化类型，从而得到和序列化时相同的对象类型
            return JsonConvert.DeserializeObject<T>(stringObject, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        /// <summary>
        /// 从Redis中删除键值，如果键值在Redis中不存在，该方法不会报错，只是什么都不会发生
        /// </summary>
        /// <param name="key">缓存键</param>
        public void Remove(string key)
        {
            _Cache.Remove(key);//如果键值在Redis中不存在，IDistributedCache.Remove方法不会报错，但是如果传入的参数key为null，则会抛出异常
        }

        /// <summary>
        /// 从Redis中刷新键值
        /// </summary>
        /// <param name="key">缓存键</param>
        public void Refresh(string key)
        {
            _Cache.Refresh(key);
        }


        #region 自定义缓存操作

        ///// <summary>
        ///// 获取缓存Key
        ///// </summary>
        ///// <param name="cacheKey">缓存类型</param> 
        ///// <returns></returns>
        //public CacheModel GetCustomKey(CacheKey cacheKey)
        //{
        //    CacheModel keyRole = _JsonCacheConfig.GetCacheModel(cacheKey);
        //    if (keyRole == null)
        //    {
        //        return null;
        //    }
        //    if (keyRole.CacheStr.IndexOf("[UserAccount]") >= 0)
        //    {

        //        keyRole.CacheStr = keyRole.CacheStr.Replace("[UserAccount]", HttpContextHelper.GetUserID());
        //    }
        //    if (keyRole.CacheStr.IndexOf("[Token]") >= 0)
        //    {
        //        keyRole.CacheStr = keyRole.CacheStr.Replace("[Token]", HttpContextHelper.GetToken());
        //    }
        //    return keyRole;
        //}

        ///// <summary>
        ///// 获取缓存Key
        ///// </summary>
        ///// <param name="cacheKey">缓存类型</param>
        ///// <param name="replaceStr">key的替换目标（原）</param>
        ///// <param name="replaceKey">key的替换部分（新）</param>
        ///// <returns></returns>
        //public CacheModel GetCustomKey(CacheKey cacheKey, string replaceStr, string replaceKey)
        //{
        //    CacheModel keyRole = _JsonCacheConfig.GetCacheModel(cacheKey);
        //    if (keyRole == null)
        //    {
        //        return null;
        //    }

        //    if (keyRole.CacheStr.Contains(replaceStr))
        //    {
        //        keyRole.CacheStr = keyRole.CacheStr.Replace(replaceStr, replaceKey);
        //    }

        //    return keyRole;
        //}

        ///// <summary>
        ///// 缓存自定义规则的缓存模式
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="cacheKey">缓存类型</param>
        ///// <param name="model">需要缓存的值</param>
        ///// <returns></returns>
        //public bool CacheCustomSetValue<T>(CacheKey cacheKey, T model) where T : class
        //{ 
        //    try
        //    {
        //        CacheModel keyRole = GetCustomKey(cacheKey);
        //        if (string.IsNullOrWhiteSpace(keyRole?.CacheStr))
        //            return false;

        //        //设置缓存
        //        SetCache(model, keyRole.CacheStr, keyRole.GetCacheTime());
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteErrorLog("自定义缓存写入失败", ex);
        //        return false;
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 缓存自定义规则的获取模式
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="cacheKey">缓存类型</param>
        ///// <returns></returns>
        //public T CacheCustomGetValue<T>(CacheKey cacheKey) where T : class
        //{ 
        //    try
        //    {
        //        CacheModel keyRole = GetCustomKey(cacheKey);
        //        if (string.IsNullOrWhiteSpace(keyRole?.CacheStr))
        //            return default(T);

        //        //获取缓存
        //        return GetCache<T>(keyRole.CacheStr);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteErrorLog("自定义缓存获取失败", ex);
        //        return default(T);
        //    }
        //}

        ///// <summary>
        ///// 缓存自定义规则的删除缓存
        ///// </summary>
        ///// <param name="cacheKey"></param>
        ///// <returns></returns>
        //public bool CacheCustomRemoveValue(CacheKey cacheKey)
        //{ 
        //    try
        //    {
        //        CacheModel keyRole = GetCustomKey(cacheKey);
        //        if (string.IsNullOrWhiteSpace(keyRole?.CacheStr))
        //            return false;

        //        //删除缓存
        //        return RemoveCache(keyRole.CacheStr);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteErrorLog("删除缓存失败", ex);
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 缓存自定义规则的缓存模式
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="cacheKey">缓存类型</param>
        ///// <param name="model">需要缓存的值</param>
        ///// <param name="keyID">key的替换部分</param>
        ///// <returns></returns>
        //public bool CacheCustomSetValue<T>(CacheKey cacheKey, T model, string keyID) where T : class
        //{ 
        //    try
        //    {
        //        CacheModel keyRole = GetCustomKey(cacheKey, keyID);
        //        if (string.IsNullOrWhiteSpace(keyRole?.CacheStr))
        //            return false;

        //        //设置缓存
        //        SetCache(model, keyRole.CacheStr, keyRole.GetCacheTime());
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteErrorLog("自定义缓存写入失败", ex);
        //        return false;
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 缓存自定义规则的获取模式
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="cacheKey">缓存类型</param>     
        ///// <param name="keyID">key的替换部分</param>
        ///// <returns></returns>
        //public T CacheCustomGetValue<T>(CacheKey cacheKey, string keyID) where T : class
        //{ 
        //    try
        //    {
        //        CacheModel keyRole = GetCustomKey(cacheKey, keyID);
        //        if (string.IsNullOrWhiteSpace(keyRole?.CacheStr))
        //            return default(T);

        //        //获取缓存
        //        return GetCache<T>(keyRole.CacheStr);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteErrorLog("自定义缓存获取失败", ex);
        //        return default(T);
        //    }
        //}

        ///// <summary>
        ///// 缓存自定义规则的删除缓存
        ///// </summary>
        ///// <param name="cacheKey"></param>
        ///// <param name="keyID">key的替换部分</param>
        ///// <returns></returns>
        //public bool CacheCustomRemoveValue(CacheKey cacheKey, string keyID)
        //{ 
        //    try
        //    {
        //        CacheModel keyRole = GetCustomKey(cacheKey, keyID);
        //        if (string.IsNullOrWhiteSpace(keyRole?.CacheStr))
        //            return false;

        //        //删除缓存
        //        return RemoveCache(keyRole.CacheStr);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteErrorLog("删除缓存失败", ex);
        //        return false;
        //    }
        //}

        #endregion
    }
}