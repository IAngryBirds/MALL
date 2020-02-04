using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILBLI.Redis
{
    /// <summary>
    /// key-value 键值对:value可以是序列化的数据
    /// </summary>
    public class RedisStringService : RedisBaseService
    {
        #region 构造函数

        /// <summary>
        /// 初始化Redis的String数据结构操作
        /// </summary>
        /// <param name="dbNum">操作的数据库索引0-64(需要在conf文件中配置)</param> 
        public RedisStringService(string connectionString, int defaultDB=0) :
           base(connectionString, defaultDB)
        { }
        #endregion

        #region 同步方法
        /// <summary>
        /// 添加单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        { 
            return _conn.GetDatabase(_DefaultDB).StringSet(key, value, expiry);
        }

        /// <summary>
        /// 添加多个key/value
        /// </summary>
        /// <param name="valueList">key/value集合</param>
        /// <returns></returns>
        public bool StringSet(Dictionary<string, string> valueList)
        {
            var newkeyValues = valueList.Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, p.Value)).ToArray();
            return _conn.GetDatabase(_DefaultDB).StringSet(newkeyValues);
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">保存的Key名称</param>
        /// <param name="value">对象实体</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        { 
            string jsonValue = ConvertJson(value);
            return _conn.GetDatabase(_DefaultDB).StringSet(key, jsonValue, expiry);
        }

        /// <summary>
        /// 在原有key的value值之后追加value
        /// </summary>
        /// <param name="key">追加的Key名称</param>
        /// <param name="value">追加的值</param>
        /// <returns></returns>
        public long StringAppend(string key, string value)
        { 
            return _conn.GetDatabase(_DefaultDB).StringAppend(key, value);
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">要读取的Key名称</param>
        /// <returns></returns>
        public string StringGet(string key)
        { 
            return _conn.GetDatabase(_DefaultDB).StringGet(key);
        }

        /// <summary>
        /// 获取多个key的value值
        /// </summary>
        /// <param name="keys">要获取值的Key集合</param>
        /// <returns></returns>
        public List<string> StringGet(params string[] keys)
        {
            var newKeys = ConvertRedisKeys(keys);
            var values = _conn.GetDatabase(_DefaultDB).StringGet(newKeys);
            return values.Select(o => o.ToString()).ToList();
        }


        /// <summary>
        /// 获取单个key的value值
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="key">要获取值的Key集合</param>
        /// <returns></returns>
        public T StringGet<T>(string key)
        { 
            var values = _conn.GetDatabase(_DefaultDB).StringGet(key);
            return ConvertObj<T>(values);
        }

        /// <summary>
        /// 获取多个key的value值
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="keys">要获取值的Key集合</param>
        /// <returns></returns>
        public List<T> StringGet<T>(params string[] keys)
        {
            var newKeys = ConvertRedisKeys(keys);
            var values = _conn.GetDatabase(_DefaultDB).StringGet(newKeys);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 获取旧值赋上新值
        /// </summary>
        /// <param name="key">Key名称</param>
        /// <param name="value">新值</param>
        /// <returns></returns>
        public string StringGetSet(string key, string value)
        { 
            return _conn.GetDatabase(_DefaultDB).StringGetSet(key, value);
        }

        /// <summary>
        /// 获取旧值赋上新值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">Key名称</param>
        /// <param name="value">新值</param>
        /// <returns></returns>
        public T StringGetSet<T>(string key, T value)
        { 
            string jsonValue = ConvertJson(value);
            var oValue = _conn.GetDatabase(_DefaultDB).StringGetSet(key, jsonValue);
            return ConvertObj<T>(oValue);
        }


        /// <summary>
        /// 获取值的长度
        /// </summary>
        /// <param name="key">Key名称</param>
        /// <returns></returns>
        public long StringGetLength(string key)
        { 
            return _conn.GetDatabase(_DefaultDB).StringLength(key);
        }

        /// <summary>
        /// 数字增长val，返回自增后的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double StringIncrement(string key, double val = 1)
        { 
            return _conn.GetDatabase(_DefaultDB).StringIncrement(key, val);
        }

        /// <summary>
        /// 数字减少val，返回自减少的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double StringDecrement(string key, double val = 1)
        { 
            return _conn.GetDatabase(_DefaultDB).StringDecrement(key, val);
        }

        #endregion

        #region 异步方法
        /// <summary>
        /// 异步方法 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        { 
            return await _conn.GetDatabase(_DefaultDB).StringSetAsync(key, value, expiry);
        }
        /// <summary>
        /// 异步方法 添加多个key/value
        /// </summary>
        /// <param name="valueList">key/value集合</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(Dictionary<string, string> valueList)
        {
            var newkeyValues = valueList.Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, p.Value)).ToArray();
            return await _conn.GetDatabase(_DefaultDB).StringSetAsync(newkeyValues);
        }

        /// <summary>
        /// 异步方法 保存一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">保存的Key名称</param>
        /// <param name="obj">对象实体</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        { 
            string jsonValue = ConvertJson(obj);
            return await _conn.GetDatabase(_DefaultDB).StringSetAsync(key, jsonValue, expiry);
        }

        /// <summary>
        /// 异步方法 在原有key的value值之后追加value
        /// </summary>
        /// <param name="key">追加的Key名称</param>
        /// <param name="value">追加的值</param>
        /// <returns></returns>
        public async Task<long> StringAppendAsync(string key, string value)
        { 
            return await _conn.GetDatabase(_DefaultDB).StringAppendAsync(key, value);
        }

        /// <summary>
        /// 异步方法 获取单个key的值
        /// </summary>
        /// <param name="key">要读取的Key名称</param>
        /// <returns></returns>
        public async Task<string> StringGetAsync(string key)
        { 
            return await _conn.GetDatabase(_DefaultDB).StringGetAsync(key);
        }

        /// <summary>
        /// 异步方法 获取多个key的value值
        /// </summary>
        /// <param name="keys">要获取值的Key集合</param>
        /// <returns></returns>
        public async Task<List<string>> StringGetAsync(params string[] keys)
        {
            var newKeys = ConvertRedisKeys(keys);
            var values = await _conn.GetDatabase(_DefaultDB).StringGetAsync(newKeys);
            return values.Select(o => o.ToString()).ToList();
        }


        /// <summary>
        /// 异步方法 获取单个key的value值
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="key">要获取值的Key集合</param>
        /// <returns></returns>
        public async Task<T> StringGetAsync<T>(string key)
        { 
            var values = await _conn.GetDatabase(_DefaultDB).StringGetAsync(key);
            return ConvertObj<T>(values);
        }

        /// <summary>
        /// 异步方法 获取多个key的value值
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="keys">要获取值的Key集合</param>
        /// <returns></returns>
        public async Task<List<T>> StringGetAsync<T>(params string[] keys)
        {
            var newKeys = ConvertRedisKeys(keys);
            var values = await _conn.GetDatabase(_DefaultDB).StringGetAsync(newKeys);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 异步方法 获取旧值赋上新值
        /// </summary>
        /// <param name="key">Key名称</param>
        /// <param name="value">新值</param>
        /// <returns></returns>
        public async Task<string> StringGetSetAsync(string key, string value)
        { 
            return await _conn.GetDatabase(_DefaultDB).StringGetSetAsync(key, value);
        }

        /// <summary>
        /// 异步方法 获取旧值赋上新值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">Key名称</param>
        /// <param name="value">新值</param>
        /// <returns></returns>
        public async Task<T> StringGetSetAsync<T>(string key, T value)
        { 
            string jsonValue = ConvertJson(value);
            var oValue = await _conn.GetDatabase(_DefaultDB).StringGetSetAsync(key, jsonValue);
            return ConvertObj<T>(oValue);
        }


        /// <summary>
        /// 异步方法 获取值的长度
        /// </summary>
        /// <param name="key">Key名称</param>
        /// <returns></returns>
        public async Task<long> StringGetLengthAsync(string key)
        { 
            return await _conn.GetDatabase(_DefaultDB).StringLengthAsync(key);
        }

        /// <summary>
        /// 异步方法 数字增长val，返回自增后的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> StringIncrementAsync(string key, double val = 1)
        { 
            return await _conn.GetDatabase(_DefaultDB).StringIncrementAsync(key, val);
        }

        /// <summary>
        /// 异步方法 数字减少val，返回自减少的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> StringDecrementAsync(string key, double val = 1)
        { 
            return await _conn.GetDatabase(_DefaultDB).StringDecrementAsync(key, val);
        }

        #endregion
    }
}
