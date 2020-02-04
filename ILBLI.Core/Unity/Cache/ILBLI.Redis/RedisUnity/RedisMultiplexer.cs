using StackExchange.Redis;
using System;
using System.Collections.Concurrent;

namespace ILBLI.Redis
{
    /// <summary>
    /// 初始化ConnectionMultiplexer（单例模式）
    /// </summary>
    public class RedisMultiplexer  
    {
        public static ConnectionMultiplexer _ConnectionMultiplexer;

        private static readonly object _LockObj = new object();
         
        /// <summary>
        /// 构造函数初始化
        /// </summary>
        /// <param name="instanceName">实例名</param>
        /// <param name="connectionString">连接串</param>
        public RedisMultiplexer(string connectionString)
        {
            if (_ConnectionMultiplexer==null)
            {
                lock (_LockObj)
                {
                    if (_ConnectionMultiplexer==null)
                    {
                        _ConnectionMultiplexer = CreateMultiplexer(connectionString);
                    }
                }
            }
        }

        /// <summary>
        /// 获取Redis链接管理类实体
        /// </summary>
        /// <returns></returns>
        public ConnectionMultiplexer GetMultiplexer()
        {
            return _ConnectionMultiplexer;
        }

        /* 
         /// <summary>
         /// （Redis连接字典池）线程安全
         /// </summary>
         //private ConcurrentDictionary<string, ConnectionMultiplexer> _ConnectionMultiplexerDic=new ConcurrentDictionary<string, ConnectionMultiplexer>();

         /// <summary> 
         /// 获取一个ConnectionMultiplexer 对象
         /// </summary>
         /// <returns></returns>
         public ConnectionMultiplexer GetMultiplexer(string instanceName, string connectionString)
         {
             if (!_ConnectionMultiplexerDic.ContainsKey(instanceName))
             {
                 lock (_LockObj)
                 {
                     if (!_ConnectionMultiplexerDic.ContainsKey(instanceName))
                     {
                         _ConnectionMultiplexerDic.TryAdd(instanceName, CreateMultiplexer(connectionString));
                     }
                 }
             }
             _ConnectionMultiplexerDic.TryGetValue(instanceName, out ConnectionMultiplexer connectionMultiplexer);
             return connectionMultiplexer;
         }
         */


        /// <summary>
        /// 内部方法，获取Redis连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private ConnectionMultiplexer CreateMultiplexer(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("请配置Redis连接字符串...");
            }
            ConnectionMultiplexer connect = ConnectionMultiplexer.Connect(connectionString);
            //注册如下事件：
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;

            return connect;
        }
        
        #region 事件

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            //log.InfoAsync($"Configuration changed: {e.EndPoint}");
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            //log.InfoAsync($"ErrorMessage: {e.Message}");
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            //log.InfoAsync($"ConnectionRestored: {e.EndPoint}");
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            //log.InfoAsync($"重新连接：Endpoint failed: {e.EndPoint},  {e.FailureType} , {(e.Exception == null ? "" : e.Exception.Message)}");
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            //log.InfoAsync($"HashSlotMoved:NewEndPoint{e.NewEndPoint}, OldEndPoint{e.OldEndPoint}");
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            //log.InfoAsync($"InternalError:Message{ e.Exception.Message}");
        }

        #endregion 事件
    }
}
