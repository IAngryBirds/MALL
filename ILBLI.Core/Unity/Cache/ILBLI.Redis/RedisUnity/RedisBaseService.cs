using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILBLI.Redis
{
    /// <summary>
    /// 静态Redis管理类
    /// </summary>
    public class RedisBaseService
    { 

        /// <summary>
        /// Redis 连接管理类
        /// </summary>
        protected readonly IConnectionMultiplexer _conn;

        protected readonly int _DefaultDB;

        /// <summary>
        /// 自定义锁
        /// </summary>
        private static readonly object _LockObj = new object();
       
        public RedisBaseService(string connectionString,int defaultDB=0)
        {
            this._DefaultDB = defaultDB; 
            if (_conn == null)
            {
                lock (_LockObj)
                {
                    if (_conn == null)
                    {
                        _conn = new RedisMultiplexer(connectionString).GetMultiplexer();
                    }
                }
            }
        }

        /* 
         * #region 外部调用静态方法
         /// <summary>
         /// 获取Redis的String数据类型操作辅助方法类
         /// </summary>
         /// <returns></returns>
         public static RedisStringService StringService => new RedisStringService();
         /// <summary>
         /// 获取Redis的Hash数据类型操作辅助方法类
         /// </summary>
         /// <returns></returns>
         public static RedisHashService HashService => new RedisHashService();
         /// <summary>
         /// 获取Redis的List数据类型操作辅助方法类
         /// </summary>
         /// <returns></returns>
         public static RedisListService ListService => new RedisListService();
         /// <summary>
         /// 获取Redis的Set无序集合数据类型操作辅助方法类
         /// </summary>
         /// <returns></returns>
         public static RedisSetService SetService => new RedisSetService();
         /// <summary>
         /// 获取Redis的SortedSet(ZSet)有序集合数据类型操作辅助方法类
         /// </summary>
         /// <returns></returns>
         public static RedisSortedSetService SortedSetService => new RedisSortedSetService();
         /// <summary>
         /// 获取Redis的Sub订阅类型操作辅助方法类
         /// </summary>
         public static RedisSubService SubService => new RedisSubService();

         #endregion
         */
        #region 不建议公开这些方法，如果项目中用不到，建议注释或者删除
        ///// <summary>
        ///// 获取Redis事务对象
        ///// </summary>
        ///// <returns></returns>
        //public ITransaction CreateTransaction() => _conn.GetDatabase(_DefaultDB).CreateTransaction();

        ///// <summary>
        ///// 获取Redis服务和常用操作对象
        ///// </summary>
        ///// <returns></returns>
        //public IDatabase GetDatabase() => _conn.GetDatabase(_DefaultDB);

        ///// <summary>
        ///// 获取Redis服务
        ///// </summary>
        ///// <param name="hostAndPort"></param>
        ///// <returns></returns>
        //public IServer GetServer(string hostAndPort) => _conn.GetServer(hostAndPort);

        ///// <summary>
        ///// 执行Redis事务
        ///// </summary>
        ///// <param name="act"></param>
        ///// <returns></returns>
        //public bool RedisTransaction(Action<ITransaction> act)
        //{
        //    var tran = _conn.GetDatabase(_DefaultDB).CreateTransaction();
        //    act.Invoke(tran);
        //    bool committed = tran.Execute();
        //    return committed;
        //}
        ///// <summary>
        ///// Redis锁
        ///// </summary>
        ///// <param name="act"></param>
        ///// <param name="ts">锁住时间</param>
        //public void RedisLockTake(Action act, TimeSpan ts)
        //{
        //    RedisValue token = Environment.MachineName;
        //    string lockKey = "lock_LockTake";
        //    if (_conn.GetDatabase(_DefaultDB).LockTake(lockKey, token, ts))
        //    {
        //        try
        //        {
        //            act();
        //        }
        //        finally
        //        {
        //            _conn.GetDatabase(_DefaultDB).LockRelease(lockKey, token);
        //        }
        //    }
        //}
        #endregion 其他


        #region 同步方法

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">要删除的key</param>
        /// <returns>是否删除成功</returns>
        public bool KeyDelete(string key)
        { 
            return _conn.GetDatabase(_DefaultDB).KeyDelete(key);
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">要删除的key集合</param>
        /// <returns>成功删除的个数</returns>
        public long KeyDelete(params string[] keys)
        {
            RedisKey[] newKeys = keys.Select(o => (RedisKey)o).ToArray();
            return _conn.GetDatabase(_DefaultDB).KeyDelete(newKeys);
        }

        /// <summary>
        /// 清空当前DataBase中所有Key
        /// </summary>
        public void KeyFulsh()
        {
            //直接执行清除命令
            _conn.GetDatabase(_DefaultDB).Execute("FLUSHDB");
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key">要判断的key</param>
        /// <returns></returns>
        public bool KeyExists(string key)
        { 
            return _conn.GetDatabase(_DefaultDB).KeyExists(key);
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public bool KeyRename(string key, string newKey)
        { 
            return _conn.GetDatabase(_DefaultDB).KeyRename(key, newKey);
        }

        /// <summary>
        /// 设置Key的过期时间
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?))
        { 
            return _conn.GetDatabase(_DefaultDB).KeyExpire(key, expiry);
        }


        #endregion

        #region 异步方法

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">要删除的key</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> KeyDeleteAsync(string key)
        { 
            return await _conn.GetDatabase(_DefaultDB).KeyDeleteAsync(key);
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">要删除的key集合</param>
        /// <returns>成功删除的个数</returns>
        public async Task<long> KeyDeleteAsync(params string[] keys)
        {
            RedisKey[] newKeys = keys.Select(o => (RedisKey)o).ToArray();
            return await _conn.GetDatabase(_DefaultDB).KeyDeleteAsync(newKeys);
        }

        /// <summary>
        /// 清空当前DataBase中所有Key
        /// </summary>
        public async Task KeyFulshAsync()
        {
            //直接执行清除命令
            await _conn.GetDatabase(_DefaultDB).ExecuteAsync("FLUSHDB");
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key">要判断的key</param>
        /// <returns></returns>
        public async Task<bool> KeyExistsAsync(string key)
        { 
            return await _conn.GetDatabase(_DefaultDB).KeyExistsAsync(key);
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public async Task<bool> KeyRenameAsync(string key, string newKey)
        { 
            return await _conn.GetDatabase(_DefaultDB).KeyRenameAsync(key, newKey);
        }

        /// <summary>
        /// 设置Key的过期时间
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> KeyExpireAsync(string key, TimeSpan? expiry = default(TimeSpan?))
        { 
            return await _conn.GetDatabase(_DefaultDB).KeyExpireAsync(key, expiry);
        }
        #endregion

        #region 辅助方法

        /// <summary>
        /// 将对象转换成string字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string ConvertJson<T>(T value)
        {
            string result = JsonConvert.SerializeObject(value, Formatting.None);
            return result;
        }
        /// <summary>
        /// 将值反系列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected T ConvertObj<T>(RedisValue value)
        {
            return value.IsNullOrEmpty ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 将值反系列化成对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        protected List<T> ConvetList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }
        /// <summary>
        /// 将string类型的Key转换成 <see cref="RedisKey"/> 型的Key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        protected RedisKey[] ConvertRedisKeys(List<string> redisKeys) => redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();

        /// <summary>
        /// 将string类型的Key转换成 <see cref="RedisKey"/> 型的Key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        protected RedisKey[] ConvertRedisKeys(params string[] redisKeys) => redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();

        /// <summary>
        /// 将值集合转换成RedisValue集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisValues"></param>
        /// <returns></returns>
        protected RedisValue[] ConvertRedisValue<T>(params T[] redisValues) => redisValues.Select(o => (RedisValue)ConvertJson<T>(o)).ToArray();
        #endregion 辅助方法

    }
}
