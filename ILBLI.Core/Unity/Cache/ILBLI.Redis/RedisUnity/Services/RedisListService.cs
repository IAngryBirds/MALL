using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ILBLI.Redis
{
    /// <summary>
    /// Redis list的实现为一个双向链表，即可以支持反向查找和遍历，更方便操作，不过带来了部分额外的内存开销，
    /// Redis内部的很多实现，包括发送缓冲队列等也都是用的这个数据结构。  
    /// 一般是左进右出或者右进左出 
    /// </summary>
    public class RedisListService : RedisBaseService
    {
        #region 构造函数

        /// <summary>
        /// 初始化Redis的List数据结构操作
        /// </summary>
        /// <param name="dbNum">操作的数据库索引0-64(需要在conf文件中配置)</param>
        public RedisListService(string connectionString, int defaultDB=0) :
            base(connectionString, defaultDB)
        { }
        #endregion

        #region 同步方法
        /// <summary>
        /// 从左侧向list中添加一个值，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListLeftPush<T>(string key, T value)
        { 
            string jValue = ConvertJson(value);
            return _conn.GetDatabase(_DefaultDB).ListLeftPush(key, jValue);
        }

        /// <summary>
        /// 从左侧向list中添加多个值，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListLeftPush<T>(string key, List<T> value)
        { 
            RedisValue[] valueList = base.ConvertRedisValue(value.ToArray());
            return _conn.GetDatabase(_DefaultDB).ListLeftPush(key, valueList);
        }

        /// <summary>
        /// 从右侧向list中添加一个值，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListRightPush<T>(string key, T value)
        { 
            string jValue = ConvertJson(value);
            return _conn.GetDatabase(_DefaultDB).ListRightPush(key, jValue);
        }

        /// <summary>
        /// 从右侧向list中添加多个值，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListRightPush<T>(string key, List<T> value)
        { 
            RedisValue[] valueList = base.ConvertRedisValue(value.ToArray());
            return _conn.GetDatabase(_DefaultDB).ListRightPush(key, valueList);
        }

        /// <summary>
        /// 从左侧向list中取出一个值并从list中删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string key)
        { 
            var rValue = _conn.GetDatabase(_DefaultDB).ListLeftPop(key);
            return base.ConvertObj<T>(rValue);
        }

        /// <summary>
        /// 从右侧向list中取出一个值并从list中删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListRightPop<T>(string key)
        { 
            var rValue = _conn.GetDatabase(_DefaultDB).ListRightPop(key);
            return base.ConvertObj<T>(rValue);
        }

        /// <summary>
        /// 从key的List中右侧取出一个值，并从左侧添加到destination集合中，且返回该数据对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">要取出数据的List名称</param>
        /// <param name="destination">要添加到的List名称</param>
        /// <returns></returns>
        public T ListRightPopLeftPush<T>(string key, string destination)
        {  
            var rValue = _conn.GetDatabase(_DefaultDB).ListRightPopLeftPush(key, destination);
            return base.ConvertObj<T>(rValue);
        }

        /// <summary>
        /// 在key的List指定值pivot之后插入value，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pivot">索引值[要插入到哪个值的后面]</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        public long ListInsertAfter<T>(string key, T pivot, T value)
        { 
            string pValue = ConvertJson(pivot);
            string jValue = ConvertJson(value);
            return _conn.GetDatabase(_DefaultDB).ListInsertAfter(key, pValue, jValue);
        }

        /// <summary>
        /// 在key的List指定值pivot之前插入value，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pivot">索引值</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        public long ListInsertBefore<T>(string key, T pivot, T value)
        { 
            string pValue = ConvertJson(pivot);
            string jValue = ConvertJson(value);
            return _conn.GetDatabase(_DefaultDB).ListInsertBefore(key, pValue, jValue);
        }

        /// <summary>
        /// 从key的list中取出所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> ListRange<T>(string key)
        { 
            var rValue = _conn.GetDatabase(_DefaultDB).ListRange(key);
            return base.ConvetList<T>(rValue);
        }

        /// <summary>
        /// 从key的List获取指定索引的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public T ListGetByIndex<T>(string key, long index)
        { 
            var rValue = _conn.GetDatabase(_DefaultDB).ListGetByIndex(key, index);
            return base.ConvertObj<T>(rValue);
        }

        /// <summary>
        /// 获取key的list中数据个数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListLength(string key)
        { 
            return _conn.GetDatabase(_DefaultDB).ListLength(key);
        }

        /// <summary>
        /// 从key的List中移除指定的值，返回删除个数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListRemove<T>(string key, T value)
        { 
            string jValue = ConvertJson(value);
            return _conn.GetDatabase(_DefaultDB).ListRemove(key, jValue);
        }
        #endregion

        #region 异步方法
        /// <summary>
        /// 从左侧向list中添加一个值，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync<T>(string key, T value)
        { 
            string jValue = ConvertJson(value);
            return await _conn.GetDatabase(_DefaultDB).ListLeftPushAsync(key, jValue);
        }

        /// <summary>
        /// 从左侧向list中添加多个值，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync<T>(string key, List<T> value)
        { 
            RedisValue[] valueList = base.ConvertRedisValue(value.ToArray());
            return await _conn.GetDatabase(_DefaultDB).ListLeftPushAsync(key, valueList);
        }

        /// <summary>
        /// 从右侧向list中添加一个值，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync<T>(string key, T value)
        { 
            string jValue = ConvertJson(value);
            return await _conn.GetDatabase(_DefaultDB).ListRightPushAsync(key, jValue);
        }

        /// <summary>
        /// 从右侧向list中添加多个值，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync<T>(string key, List<T> value)
        { 
            RedisValue[] valueList = base.ConvertRedisValue(value.ToArray());
            return await _conn.GetDatabase(_DefaultDB).ListRightPushAsync(key, valueList);
        }

        /// <summary>
        /// 从左侧向list中取出一个值并从list中删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string key)
        { 
            var rValue = await _conn.GetDatabase(_DefaultDB).ListLeftPopAsync(key);
            return base.ConvertObj<T>(rValue);
        }

        /// <summary>
        /// 从右侧向list中取出一个值并从list中删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string key)
        { 
            var rValue = await _conn.GetDatabase(_DefaultDB).ListRightPopAsync(key);
            return base.ConvertObj<T>(rValue);
        }

        /// <summary>
        /// 从key的List中右侧取出一个值，并从左侧添加到destination集合中，且返回该数据对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">要取出数据的List名称</param>
        /// <param name="destination">要添加到的List名称</param>
        /// <returns></returns>
        public async Task<T> ListRightPopLeftPushAsync<T>(string key, string destination)
        { 
            var rValue = await _conn.GetDatabase(_DefaultDB).ListRightPopLeftPushAsync(key, destination);
            return base.ConvertObj<T>(rValue);
        }

        /// <summary>
        /// 在key的List指定值pivot之后插入value，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pivot">索引值</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        public async Task<long> ListInsertAfterAsync<T>(string key, T pivot, T value)
        { 
            string pValue = ConvertJson(pivot);
            string jValue = ConvertJson(value);
            return await _conn.GetDatabase(_DefaultDB).ListInsertAfterAsync(key, pValue, jValue);
        }

        /// <summary>
        /// 在key的List指定值pivot之前插入value，返回集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pivot">索引值</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        public async Task<long> ListInsertBeforeAsync<T>(string key, T pivot, T value)
        { 
            string pValue = ConvertJson(pivot);
            string jValue = ConvertJson(value);
            return await _conn.GetDatabase(_DefaultDB).ListInsertBeforeAsync(key, pValue, jValue);
        }

        /// <summary>
        /// 从key的list中取出所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> ListRangeAsync<T>(string key)
        { 
            var rValue = await _conn.GetDatabase(_DefaultDB).ListRangeAsync(key);
            return base.ConvetList<T>(rValue);
        }

        /// <summary>
        /// 从key的List获取指定索引的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<T> ListGetByIndexAsync<T>(string key, long index)
        { 
            var rValue = await _conn.GetDatabase(_DefaultDB).ListGetByIndexAsync(key, index);
            return base.ConvertObj<T>(rValue);
        }

        /// <summary>
        /// 获取key的list中数据个数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string key)
        { 
            return await _conn.GetDatabase(_DefaultDB).ListLengthAsync(key);
        }

        /// <summary>
        /// 从key的List中移除指定的值，返回删除个数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> ListRemoveAsync<T>(string key, T value)
        { 
            string jValue = ConvertJson(value);
            return await _conn.GetDatabase(_DefaultDB).ListRemoveAsync(key, jValue);
        }
        #endregion

    }
}
