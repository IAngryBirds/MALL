using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace ILBLI.Core
{
    public class RedisExConfig
    {
        /// <summary>
        /// 需要配置Redis的配置文件信息
        /// </summary>
        private static Dictionary<RedisEnum, ConnectionMultiplexer> dicRedisConnect = new Dictionary<RedisEnum, ConnectionMultiplexer>();
        private static readonly object Locker = new object();


        public static ConnectionMultiplexer CreateRedisConnection(RedisEnum redisType)
        {
            ConnectionMultiplexer manager;
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
                            return manager;
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
                return dicRedisConnect[redisType];
            }
            return default(ConnectionMultiplexer);
        }

        /// <summary>
        /// 根据配置文件创建一个Redis实例
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static ConnectionMultiplexer CreateManager(RedisConfigModel model)
        {
            ConfigurationOptions redisClientManagerConfig = new ConfigurationOptions();

            if (model.RedisMasterList != null)
            {
                model.RedisMasterList.ForEach(master =>
                {
                    redisClientManagerConfig.EndPoints.Add($"{master.RedisAddress}:{master.RedisPort}");
                    redisClientManagerConfig.Password = master.RedisPassWord;
                    redisClientManagerConfig.AbortOnConnectFail = false;
                });
            }

            ConnectionMultiplexer connect=  ConnectionMultiplexer.Connect(redisClientManagerConfig);
            //注册如下事件
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;
            return connect;
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

        #region 事件

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            throw new Exception("Configuration changed: " + e.EndPoint);
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
           throw new Exception("ErrorMessage: " + e.Message);
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
           throw new Exception("ConnectionRestored: " + e.EndPoint);
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            throw new Exception("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
           throw new Exception("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            throw new Exception("InternalError:Message" + e.Exception.Message);
        }

        #endregion 事件

    }
}
