using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace ILBLI.Core
{
    public class RedisConfig
    {
        /// <summary>
        /// 需要配置Redis的配置文件信息
        /// </summary>
        private static Dictionary<RedisEnum, PooledRedisClientManager> dicRedisConnect = new Dictionary<RedisEnum, PooledRedisClientManager>();
        private static readonly object Locker = new object();

        public static PooledRedisClientManager CreateRedisConnection(RedisEnum redisType)
        {
            PooledRedisClientManager manager = new PooledRedisClientManager();
            if (!dicRedisConnect.ContainsKey(redisType))
            {
                lock (Locker)
                {
                    List<RedisConfigModel> configs = ReadRedisListFromXML();
                    RedisConfigModel model = configs.Find(x => x.RedisConfigCode == redisType.ToString());
                    if (model != null)
                    {

                        if (!dicRedisConnect.ContainsKey(redisType))
                        {
                            manager = CreateManager(model);
                            dicRedisConnect.Add(redisType, manager);
                        }
                    }
                }
                if (dicRedisConnect.ContainsKey(redisType))
                {
                    return dicRedisConnect[redisType];
                }
            }
            else
            {
                manager = dicRedisConnect[redisType];
            }
            return manager;
        }
        
        /// <summary>
        /// 根据配置文件创建一个Redis实例
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static PooledRedisClientManager CreateManager(RedisConfigModel model) 
        {
            RedisClientManagerConfig redisClientManagerConfig = new RedisClientManagerConfig();
            redisClientManagerConfig.AutoStart = model.AutoStart;
            redisClientManagerConfig.MaxReadPoolSize = model.MaxReadPoolSize;
            redisClientManagerConfig.MaxWritePoolSize = model.MaxWritePoolSize;
            List<string> readWriteHosts = new List<string>();
            List<string> readOnlyHosts = new List<string>();
            if (model.RedisMasterList != null)
            {
                model.RedisMasterList.ForEach(master =>
                {
                    readWriteHosts.Add($"{master.RedisPassWord}@{master.RedisAddress}:{master.RedisPort}");
                });
            }
            if (model.RedisSlaveList != null)
            {
                model.RedisMasterList.ForEach(slave =>
                {
                    readOnlyHosts.Add($"{slave.RedisPassWord}@{slave.RedisAddress}:{slave.RedisPort}");
                });
            }
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, redisClientManagerConfig);
        }

        /// <summary>
        /// 从RedisConfig配置文件读取Redis的配置信息
        /// </summary>
        /// <returns></returns>
        public static List<RedisConfigModel> ReadRedisListFromXML()
        {
            string xmlPath = @"Config\RedisConfig.xml";
            var nodeList = XMLManager.ReadXml(xmlPath);
            List<RedisConfigModel> modelList = XMLManager.DeserializeToObject<List<RedisConfigModel>>(nodeList);
            if (modelList == null || modelList.Count == 0)
            {
                throw new Exception("尚未配置RedisConfig配置文件，请先配置");
            }
            return modelList;
        }
    }
}
