﻿using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILBLI.Redis
{
    /// <summary>
    /// Sorted Sets是将 Set 中的元素增加了一个权重参数 score，使得集合中的元素能够按 score 进行有序排列
    /// 1.带有权重的元素，比如一个游戏的用户得分排行榜
    /// 2.比较复杂的数据结构，一般用到的场景不算太多
    /// </summary>
    public class RedisSortedSetService : RedisBaseService
    {
        #region 构造函数

        /// <summary>
        /// 初始化Redis的SortedSet有序数据结构操作
        /// </summary>
        /// <param name="dbNum">操作的数据库索引0-64(需要在conf文件中配置)</param>
        public RedisSortedSetService(string connectionString, int defaultDB=0) :
            base(connectionString, defaultDB)
        { }
        #endregion

        #region 同步方法

        /// <summary>
        /// 添加一个值到Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score">排序分数，为空将获取集合中最大score加1</param>
        /// <returns></returns>
        public bool SortedSetAdd<T>(string key, T value, double? score = null)
        { 
            double scoreNum = score ?? _GetScore(key);
            return _conn.GetDatabase(_DefaultDB).SortedSetAdd(key, ConvertJson<T>(value), scoreNum);
        }

        /// <summary>
        /// 添加一个集合到Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score">排序分数，为空将获取集合中最大score加1</param>
        /// <returns></returns>
        public long SortedSetAdd<T>(string key, List<T> value, double? score = null)
        { 
            double scoreNum = score ?? _GetScore(key);
            SortedSetEntry[] rValue = value.Select(o => new SortedSetEntry(ConvertJson<T>(o), scoreNum++)).ToArray();
            return _conn.GetDatabase(_DefaultDB).SortedSetAdd(key, rValue);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SortedSetLength(string key)
        { 
            return _conn.GetDatabase(_DefaultDB).SortedSetLength(key);
        }

        /// <summary>
        /// 获取指定起始值到结束值的集合数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="startValue">起始值</param>
        /// <param name="endValue">结束值</param>
        /// <returns></returns>
        public long SortedSetLengthByValue<T>(string key, T startValue, T endValue)
        { 
            var sValue = ConvertJson<T>(startValue);
            var eValue = ConvertJson<T>(endValue);
            return _conn.GetDatabase(_DefaultDB).SortedSetLengthByValue(key, sValue, eValue);
        }

        /// <summary>
        /// 获取指定Key的排序Score值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public double? SortedSetScore<T>(string key, T value)
        { 
            var rValue = ConvertJson<T>(value);
            return _conn.GetDatabase(_DefaultDB).SortedSetScore(key, rValue);
        }

        /// <summary>
        /// 获取指定Key中最小Score值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double SortedSetMinScore(string key)
        { 
            double dValue = 0;
            var rValue = _conn.GetDatabase(_DefaultDB).SortedSetRangeByRankWithScores(key, 0, 0, Order.Ascending).FirstOrDefault();
            dValue = rValue != null ? rValue.Score : 0;
            return dValue;
        }

        /// <summary>
        /// 获取指定Key中最大Score值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double SortedSetMaxScore(string key)
        { 
            double dValue = 0;
            var rValue = _conn.GetDatabase(_DefaultDB).SortedSetRangeByRankWithScores(key, 0, 0, Order.Descending).FirstOrDefault();
            dValue = rValue != null ? rValue.Score : 0;
            return dValue;
        }

        /// <summary>
        /// 删除Key中指定的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public long SortedSetRemove<T>(string key, params T[] value)
        { 
            var rValue = ConvertRedisValue<T>(value);
            return _conn.GetDatabase(_DefaultDB).SortedSetRemove(key, rValue);
        }

        /// <summary>
        /// 删除指定起始值到结束值的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="startValue">起始值</param>
        /// <param name="endValue">结束值</param>
        /// <returns></returns>
        public long SortedSetRemoveRangeByValue<T>(string key, T startValue, T endValue)
        { 
            var sValue = ConvertJson<T>(startValue);
            var eValue = ConvertJson<T>(endValue);
            return _conn.GetDatabase(_DefaultDB).SortedSetRemoveRangeByValue(key, sValue, eValue);
        }

        /// <summary>
        /// 删除 从 start 开始的 stop 条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public long SortedSetRemoveRangeByRank(string key, long start, long stop)
        { 
            return _conn.GetDatabase(_DefaultDB).SortedSetRemoveRangeByRank(key, start, stop);
        }

        /// <summary>
        /// 根据排序分数Score，删除从 start 开始的 stop 条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public long SortedSetRemoveRangeByScore(string key, double start, double stop)
        { 
            return _conn.GetDatabase(_DefaultDB).SortedSetRemoveRangeByScore(key, start, stop);
        }

        /// <summary>
        /// 获取从 start 开始的 stop 条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">起始数</param>
        /// <param name="stop">-1表示到结束，0为1条</param>
        /// <param name="desc">是否按降序排列</param>
        /// <returns></returns>
        public List<T> SortedSetRangeByRank<T>(string key, long start = 0, long stop = -1, bool desc = false)
        { 
            Order orderBy = desc ? Order.Descending : Order.Ascending;
            var rValue = _conn.GetDatabase(_DefaultDB).SortedSetRangeByRank(key, start, stop, orderBy);
            return ConvetList<T>(rValue);
        }

        /// <summary>
        /// 获取从 start 开始的 stop 条数据包含Score，返回数据格式：Key=值，Value = Score
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">起始数</param>
        /// <param name="stop">-1表示到结束，0为1条</param>
        /// <param name="desc">是否按降序排列</param>
        /// <returns></returns>
        public Dictionary<T, double> SortedSetRangeByRankWithScores<T>(string key, long start = 0, long stop = -1, bool desc = false)
        { 
            Order orderBy = desc ? Order.Descending : Order.Ascending;
            var rValue = _conn.GetDatabase(_DefaultDB).SortedSetRangeByRankWithScores(key, start, stop, orderBy);
            Dictionary<T, double> dicList = new Dictionary<T, double>();
            foreach (var item in rValue)
            {
                dicList.Add(ConvertObj<T>(item.Element), item.Score);
            }
            return dicList;
        }

        /// <summary>
        ///  根据Score排序 获取从 start 开始的 stop 条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">起始数</param>
        /// <param name="stop">-1表示到结束，0为1条</param>
        /// <param name="desc">是否按降序排列</param>
        /// <returns></returns>
        public List<T> SortedSetRangeByScore<T>(string key, double start = 0, double stop = -1, bool desc = false)
        { 
            Order orderBy = desc ? Order.Descending : Order.Ascending;
            var rValue = _conn.GetDatabase(_DefaultDB).SortedSetRangeByScore(key, start, stop, Exclude.None, orderBy);
            return ConvetList<T>(rValue);
        }

        /// <summary>
        /// 根据Score排序  获取从 start 开始的 stop 条数据包含Score，返回数据格式：Key=值，Value = Score
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">起始数</param>
        /// <param name="stop">-1表示到结束，0为1条</param>
        /// <param name="desc">是否按降序排列</param>
        /// <returns></returns>
        public Dictionary<T, double> SortedSetRangeByScoreWithScores<T>(string key, double start = 0, double stop = -1, bool desc = false)
        { 
            Order orderBy = desc ? Order.Descending : Order.Ascending;
            var rValue = _conn.GetDatabase(_DefaultDB).SortedSetRangeByScoreWithScores(key, start, stop, Exclude.None, orderBy);
            Dictionary<T, double> dicList = new Dictionary<T, double>();
            foreach (var item in rValue)
            {
                dicList.Add(ConvertObj<T>(item.Element), item.Score);
            }
            return dicList;
        }

        /// <summary>
        /// 获取指定起始值到结束值的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="startValue">起始值</param>
        /// <param name="endValue">结束值</param>
        /// <returns></returns>
        public List<T> SortedSetRangeByValue<T>(string key, T startValue, T endValue)
        { 
            var sValue = ConvertJson<T>(startValue);
            var eValue = ConvertJson<T>(endValue);
            var rValue = _conn.GetDatabase(_DefaultDB).SortedSetRangeByValue(key, sValue, eValue);
            return ConvetList<T>(rValue);
        }

        /// <summary>
        /// 获取几个集合的并集,并保存到一个新Key中
        /// </summary>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public long SortedSetCombineUnionAndStore(string destination, params string[] keys)
        {
            return _SortedSetCombineAndStore(SetOperation.Union, destination, keys);
        }

        /// <summary>
        /// 获取几个集合的交集,并保存到一个新Key中
        /// </summary>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public long SortedSetCombineIntersectAndStore(string destination, params string[] keys)
        {
            return _SortedSetCombineAndStore(SetOperation.Intersect, destination, keys);
        }


        //交集似乎并不支持
        ///// <summary>
        ///// 获取几个集合的差集,并保存到一个新Key中
        ///// </summary>
        ///// <param name="destination">保存的新Key名称</param>
        ///// <param name="keys">要操作的Key集合</param>
        ///// <returns></returns>
        //public long SortedSetCombineDifferenceAndStore(string destination, params string[] keys)
        //{
        //    return _SortedSetCombineAndStore(SetOperation.Difference, destination, keys);
        //}



        /// <summary>
        /// 修改指定Key和值的Scores在原值上减去scores，并返回最终Scores
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="scores"></param>
        /// <returns></returns>
        public double SortedSetDecrement<T>(string key, T value, double scores)
        { 
            var rValue = ConvertJson<T>(value);
            return _conn.GetDatabase(_DefaultDB).SortedSetDecrement(key, rValue, scores);
        }

        /// <summary>
        /// 修改指定Key和值的Scores在原值上增加scores，并返回最终Scores
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="scores"></param>
        /// <returns></returns>
        public double SortedSetIncrement<T>(string key, T value, double scores)
        { 
            var rValue = ConvertJson<T>(value);
            return _conn.GetDatabase(_DefaultDB).SortedSetIncrement(key, rValue, scores);
        }



        #endregion

        #region 异步方法

        /// <summary>
        /// 添加一个值到Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score">排序分数，为空将获取集合中最大score加1</param>
        /// <returns></returns>
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double? score = null)
        { 
            double scoreNum = score ?? _GetScore(key);
            return await _conn.GetDatabase(_DefaultDB).SortedSetAddAsync(key, ConvertJson<T>(value), scoreNum);
        }

        /// <summary>
        /// 添加一个集合到Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score">排序分数，为空将获取集合中最大score加1</param>
        /// <returns></returns>
        public async Task<long> SortedSetAddAsync<T>(string key, List<T> value, double? score = null)
        { 
            double scoreNum = score ?? _GetScore(key);
            SortedSetEntry[] rValue = value.Select(o => new SortedSetEntry(ConvertJson<T>(o), scoreNum++)).ToArray();
            return await _conn.GetDatabase(_DefaultDB).SortedSetAddAsync(key, rValue);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthAsync(string key)
        { 
            return await _conn.GetDatabase(_DefaultDB).SortedSetLengthAsync(key);
        }

        /// <summary>
        /// 获取指定起始值到结束值的集合数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="startValue">起始值</param>
        /// <param name="endValue">结束值</param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthByValueAsync<T>(string key, T startValue, T endValue)
        { 
            var sValue = ConvertJson<T>(startValue);
            var eValue = ConvertJson<T>(endValue);
            return await _conn.GetDatabase(_DefaultDB).SortedSetLengthByValueAsync(key, sValue, eValue);
        }

        /// <summary>
        /// 获取指定Key的排序Score值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<double?> SortedSetScoreAsync<T>(string key, T value)
        { 
            var rValue = ConvertJson<T>(value);
            return await _conn.GetDatabase(_DefaultDB).SortedSetScoreAsync(key, rValue);
        }

        /// <summary>
        /// 获取指定Key中最小Score值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<double> SortedSetMinScoreAsync(string key)
        { 
            double dValue = 0;
            var rValue = (await _conn.GetDatabase(_DefaultDB).SortedSetRangeByRankWithScoresAsync(key, 0, 0, Order.Ascending)).FirstOrDefault();
            dValue = rValue != null ? rValue.Score : 0;
            return dValue;
        }

        /// <summary>
        /// 获取指定Key中最大Score值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<double> SortedSetMaxScoreAsync(string key)
        { 
            double dValue = 0;
            var rValue = (await _conn.GetDatabase(_DefaultDB).SortedSetRangeByRankWithScoresAsync(key, 0, 0, Order.Descending)).FirstOrDefault();
            dValue = rValue != null ? rValue.Score : 0;
            return dValue;
        }

        /// <summary>
        /// 删除Key中指定的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> SortedSetRemoveAsync<T>(string key, params T[] value)
        { 
            var rValue = ConvertRedisValue<T>(value);
            return await _conn.GetDatabase(_DefaultDB).SortedSetRemoveAsync(key, rValue);
        }

        /// <summary>
        /// 删除指定起始值到结束值的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="startValue">起始值</param>
        /// <param name="endValue">结束值</param>
        /// <returns></returns>
        public async Task<long> SortedSetRemoveRangeByValueAsync<T>(string key, T startValue, T endValue)
        { 
            var sValue = ConvertJson<T>(startValue);
            var eValue = ConvertJson<T>(endValue);
            return await _conn.GetDatabase(_DefaultDB).SortedSetRemoveRangeByValueAsync(key, sValue, eValue);
        }

        /// <summary>
        /// 删除 从 start 开始的 stop 条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public async Task<long> SortedSetRemoveRangeByRankAsync(string key, long start, long stop)
        { 
            return await _conn.GetDatabase(_DefaultDB).SortedSetRemoveRangeByRankAsync(key, start, stop);
        }

        /// <summary>
        /// 根据排序分数Score，删除从 start 开始的 stop 条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public async Task<long> SortedSetRemoveRangeByScoreAsync(string key, double start, double stop)
        { 
            return await _conn.GetDatabase(_DefaultDB).SortedSetRemoveRangeByScoreAsync(key, start, stop);
        }

        /// <summary>
        /// 获取从 start 开始的 stop 条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">起始数</param>
        /// <param name="stop">-1表示到结束，0为1条</param>
        /// <param name="desc">是否按降序排列</param>
        /// <returns></returns>
        public async Task<List<T>> SortedSetRangeByRankAsync<T>(string key, long start = 0, long stop = -1, bool desc = false)
        { 
            Order orderBy = desc ? Order.Descending : Order.Ascending;
            var rValue = await _conn.GetDatabase(_DefaultDB).SortedSetRangeByRankAsync(key, start, stop, orderBy);
            return ConvetList<T>(rValue);
        }

        /// <summary>
        /// 获取从 start 开始的 stop 条数据包含Score，返回数据格式：Key=值，Value = Score
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">起始数</param>
        /// <param name="stop">-1表示到结束，0为1条</param>
        /// <param name="desc">是否按降序排列</param>
        /// <returns></returns>
        public async Task<Dictionary<T, double>> SortedSetRangeByRankWithScoresAsync<T>(string key, long start = 0, long stop = -1, bool desc = false)
        { 
            Order orderBy = desc ? Order.Descending : Order.Ascending;
            var rValue = await _conn.GetDatabase(_DefaultDB).SortedSetRangeByRankWithScoresAsync(key, start, stop, orderBy);
            Dictionary<T, double> dicList = new Dictionary<T, double>();
            foreach (var item in rValue)
            {
                dicList.Add(ConvertObj<T>(item.Element), item.Score);
            }
            return dicList;
        }

        /// <summary>
        ///  根据Score排序 获取从 start 开始的 stop 条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">起始数</param>
        /// <param name="stop">-1表示到结束，0为1条</param>
        /// <param name="desc">是否按降序排列</param>
        /// <returns></returns>
        public async Task<List<T>> SortedSetRangeByScoreAsync<T>(string key, double start = 0, double stop = -1, bool desc = false)
        { 
            Order orderBy = desc ? Order.Descending : Order.Ascending;
            var rValue = await _conn.GetDatabase(_DefaultDB).SortedSetRangeByScoreAsync(key, start, stop, Exclude.None, orderBy);
            return ConvetList<T>(rValue);
        }

        /// <summary>
        /// 根据Score排序  获取从 start 开始的 stop 条数据包含Score，返回数据格式：Key=值，Value = Score
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">起始数</param>
        /// <param name="stop">-1表示到结束，0为1条</param>
        /// <param name="desc">是否按降序排列</param>
        /// <returns></returns>
        public async Task<Dictionary<T, double>> SortedSetRangeByScoreWithScoresAsync<T>(string key, double start = 0, double stop = -1, bool desc = false)
        { 
            Order orderBy = desc ? Order.Descending : Order.Ascending;
            var rValue = await _conn.GetDatabase(_DefaultDB).SortedSetRangeByScoreWithScoresAsync(key, start, stop, Exclude.None, orderBy);
            Dictionary<T, double> dicList = new Dictionary<T, double>();
            foreach (var item in rValue)
            {
                dicList.Add(ConvertObj<T>(item.Element), item.Score);
            }
            return dicList;
        }

        /// <summary>
        /// 获取指定起始值到结束值的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="startValue">起始值</param>
        /// <param name="endValue">结束值</param>
        /// <returns></returns>
        public async Task<List<T>> SortedSetRangeByValueAsync<T>(string key, T startValue, T endValue)
        { 
            var sValue = ConvertJson<T>(startValue);
            var eValue = ConvertJson<T>(endValue);
            var rValue = await _conn.GetDatabase(_DefaultDB).SortedSetRangeByValueAsync(key, sValue, eValue);
            return ConvetList<T>(rValue);
        }

        /// <summary>
        /// 获取几个集合的并集,并保存到一个新Key中
        /// </summary>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public async Task<long> SortedSetCombineUnionAndStoreAsync(string destination, params string[] keys)
        {
            return await _SortedSetCombineAndStoreAsync(SetOperation.Union, destination, keys);
        }

        /// <summary>
        /// 获取几个集合的交集,并保存到一个新Key中
        /// </summary>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public async Task<long> SortedSetCombineIntersectAndStoreAsync(string destination, params string[] keys)
        {
            return await _SortedSetCombineAndStoreAsync(SetOperation.Intersect, destination, keys);
        }

        ///// <summary>
        ///// 获取几个集合的差集,并保存到一个新Key中
        ///// </summary>
        ///// <param name="destination">保存的新Key名称</param>
        ///// <param name="keys">要操作的Key集合</param>
        ///// <returns></returns>
        //public async Task<long> SortedSetCombineDifferenceAndStoreAsync(string destination, params string[] keys)
        //{
        //    return await _SortedSetCombineAndStoreAsync(SetOperation.Difference, destination, keys);
        //}

        /// <summary>
        /// 修改指定Key和值的Scores在原值上减去scores，并返回最终Scores
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="scores"></param>
        /// <returns></returns>
        public async Task<double> SortedSetDecrementAsync<T>(string key, T value, double scores)
        { 
            var rValue = ConvertJson<T>(value);
            return await _conn.GetDatabase(_DefaultDB).SortedSetDecrementAsync(key, rValue, scores);
        }

        /// <summary>
        /// 修改指定Key和值的Scores在原值上增加scores，并返回最终Scores
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="scores"></param>
        /// <returns></returns>
        public async Task<double> SortedSetIncrementAsync<T>(string key, T value, double scores)
        { 
            var rValue = ConvertJson<T>(value);
            return await _conn.GetDatabase(_DefaultDB).SortedSetIncrementAsync(key, rValue, scores);
        }



        #endregion

        #region 内部辅助方法
        /// <summary>
        /// 获取指定Key中最大Score值,
        /// </summary>
        /// <param name="key">key名称，注意要先添加上Key前缀</param>
        /// <returns></returns>
        private double _GetScore(string key)
        {
            double dValue = 0;
            var rValue = _conn.GetDatabase(_DefaultDB).SortedSetRangeByRankWithScores(key, 0, 0, Order.Descending).FirstOrDefault();
            dValue = rValue != null ? rValue.Score : 0;
            return dValue + 1;
        }

        /// <summary>
        /// 获取几个集合的交叉并集合,并保存到一个新Key中
        /// </summary>
        /// <param name="operation">Union：并集  Intersect：交集  Difference：差集  详见 <see cref="SetOperation"/></param>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        private long _SortedSetCombineAndStore(SetOperation operation, string destination, params string[] keys)
        {
            #region 查看源码，似乎并不支持Difference
            //RedisCommand command;
            //if (operation != SetOperation.Union)
            //{
            //    if (operation != SetOperation.Intersect)
            //    {
            //        throw new ArgumentOutOfRangeException("operation");
            //    }
            //    command = RedisCommand.ZINTERSTORE;
            //}
            //else
            //{
            //    command = RedisCommand.ZUNIONSTORE;
            //}
            #endregion
             
            RedisKey[] keyList = base.ConvertRedisKeys(keys);
            var rValue = _conn.GetDatabase(_DefaultDB).SortedSetCombineAndStore(operation, destination, keyList);
            return rValue;

        }

        /// <summary>
        /// 获取几个集合的交叉并集合,并保存到一个新Key中
        /// </summary>
        /// <param name="operation">Union：并集  Intersect：交集  Difference：差集  详见 <see cref="SetOperation"/></param>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        private async Task<long> _SortedSetCombineAndStoreAsync(SetOperation operation, string destination, params string[] keys)
        { 
            RedisKey[] keyList = base.ConvertRedisKeys(keys);
            var rValue = await _conn.GetDatabase(_DefaultDB).SortedSetCombineAndStoreAsync(operation, destination, keyList);
            return rValue;
        }

        #endregion

    }
}
