//using RabbitMQ.Client;
//using System;
//using System.Collections.Concurrent;
//using System.IO;
//using System.Text;
//using System.Threading;
//using System.Web;
//using System.Web.Caching;

//namespace ILBLI.Core
//{
//    public class RabbitMQManager
//    {
//        private const string ConfigName = "Web.config";

//        private const string CacheKey_MQConnectionSetting = "MQConnectionSetting";
//        private const string CacheKey_MQMaxConnectionCount = "MQMaxConnectionCount";//最大连接数

//        private readonly static ConcurrentQueue<IConnection> FreeConnectionQueue;//空闲连接对象队列
//        private readonly static ConcurrentDictionary<IConnection, bool> BusyConnectionDic;//使用中（忙）连接对象集合 //为何这里使用字典对象呢，因为当我用完后，需要能够快速的找出使用中的连接对象，并能快速移出，同时重新放入到空闲队列
//        private readonly static ConcurrentDictionary<IConnection, int> MQConnectionPoolUsingDicNew;//连接池使用率  //连接使用次数记录集合，这个只是辅助记录连接使用次数，以便可以计算一个连接的已使用次数，当达到最大使用次数时，则应断开重新创建
//        private readonly static Semaphore MQConnectionPoolSemaphore; //这个是信号量，这是控制并发连接的重要手段，连接池的容量等同于这个信号量的最大可并行数，保证同时使用的连接数不超过连接池的容量，若超过则会等待
//        private readonly static object freeConnLock = new object(), addConnLock = new object();
//        private static int connCount = 0;

//        public const int DefaultMaxConnectionCount = 30;//默认最大保持可用连接数
//        public const int DefaultMaxConnectionUsingCount = 10000;//默认最大连接可访问次数


//        static RabbitMQManager()
//        {
//            FreeConnectionQueue = new ConcurrentQueue<IConnection>();//空闲队列
//            BusyConnectionDic = new ConcurrentDictionary<IConnection, bool>();//使用中得队列
//            MQConnectionPoolUsingDicNew = new ConcurrentDictionary<IConnection, int>();//连接池使用率【使用次数】
//            MQConnectionPoolSemaphore = new Semaphore(MaxConnectionCount, MaxConnectionCount, "MQConnectionPoolSemaphore");//信号量，控制同时并发可用线程数//保证同时使用的连接数不超过连接池的容量，若超过则会等待；

//        }



//        /// <summary>
//        /// 读取配置文件的额最大连接数---赋值给默认的最大可用连接数
//        /// </summary>
//        private static int MaxConnectionCount
//        {
//            get
//            {
//                if (HttpRuntime.Cache[CacheKey_MQMaxConnectionCount] != null)
//                {
//                    return Convert.ToInt32(HttpRuntime.Cache[CacheKey_MQMaxConnectionCount]);
//                }
//                else
//                {
//                    int mqMaxConnectionCount = 0;
//                    string mqMaxConnectionCountStr = RabbitConfigString.MQMaxConnectionCount;
//                    if (!int.TryParse(mqMaxConnectionCountStr, out mqMaxConnectionCount) || mqMaxConnectionCount <= 0)
//                    {
//                        mqMaxConnectionCount = DefaultMaxConnectionCount;
//                    }

//                    string appConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigName);
//                    HttpRuntime.Cache.Insert(CacheKey_MQMaxConnectionCount, mqMaxConnectionCount, new CacheDependency(appConfigPath));

//                    return mqMaxConnectionCount;
//                }

//            }
//        }

//        /// <summary>
//        /// 建立连接Factory类
//        /// </summary>
//        /// <param name="hostName">服务器地址</param>
//        /// <param name="userName">登录账号</param>
//        /// <param name="passWord">登录密码</param>
//        /// <returns></returns>
//        private static ConnectionFactory CrateFactory()
//        {
//            var mqConnectionSetting = GetMQConnectionSetting();
//            var connectionfactory = new ConnectionFactory();
//            connectionfactory.HostName = mqConnectionSetting[0];
//            connectionfactory.UserName = mqConnectionSetting[1];
//            connectionfactory.Password = mqConnectionSetting[2];
//            if (mqConnectionSetting.Length > 3) //增加端口号
//            {
//                connectionfactory.Port = Convert.ToInt32(mqConnectionSetting[3]);
//            }
//            return connectionfactory;
//        }

//        /// <summary>
//        /// 从配置文件中读取连接字符串信息
//        /// </summary>
//        /// <returns></returns>
//        private static string[] GetMQConnectionSetting()
//        {
//            string[] mqConnectionSetting = null;
//            if (HttpRuntime.Cache[CacheKey_MQConnectionSetting] == null)
//            {
//                //MQConnectionSetting=Host IP|;userid;|;password
//                string mqConnSettingStr = RabbitConfigString.MQConnectionSetting;
//                if (!string.IsNullOrWhiteSpace(mqConnSettingStr))
//                {
//                    if (mqConnSettingStr.Contains(";|;"))
//                    {
//                        mqConnectionSetting = mqConnSettingStr.Split(new[] { ";|;" }, StringSplitOptions.RemoveEmptyEntries);
//                    }
//                }

//                if (mqConnectionSetting == null || mqConnectionSetting.Length < 3)
//                {
//                    throw new Exception("MQConnectionSetting未配置或配置不正确");
//                }

//                string appConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigName);
//                HttpRuntime.Cache.Insert(CacheKey_MQConnectionSetting, mqConnectionSetting, new CacheDependency(appConfigPath));
//            }
//            else
//            {
//                mqConnectionSetting = HttpRuntime.Cache[CacheKey_MQConnectionSetting] as string[];
//            }

//            return mqConnectionSetting;
//        }

//        /// <summary>
//        /// 获取一个TCP连接
//        /// </summary>
//        /// <returns></returns>
//        public static IConnection CreateMQConnection()
//        {
//            var factory = CrateFactory();
//            factory.AutomaticRecoveryEnabled = true;//自动重连
//            var connection = factory.CreateConnection();
//            return connection;
//        }


//        /// <summary>
//        /// 从连接池中过去一个TCP连接
//        /// </summary>
//        /// <returns></returns>
//        public static IConnection CreateMQConnectionInPoolNew()
//        {

//        SelectMQConnectionLine:

//            MQConnectionPoolSemaphore.WaitOne();//当<MaxConnectionCount时，会直接进入，否则会等待直到空闲连接出现

//            IConnection mqConnection = null;
//            if (FreeConnectionQueue.Count + BusyConnectionDic.Count < MaxConnectionCount)//如果已有连接数小于最大可用连接数，则直接创建新连接
//            {
//                lock (addConnLock)
//                {
//                    if (FreeConnectionQueue.Count + BusyConnectionDic.Count < MaxConnectionCount)
//                    {
//                        mqConnection = CreateMQConnection();
//                        BusyConnectionDic[mqConnection] = true;//加入到忙连接集合中
//                        MQConnectionPoolUsingDicNew[mqConnection] = 1;//当前连接已使用次数【放回池子重新获取算一次，初始值为1次】
//                        return mqConnection;
//                    }
//                }
//            }

//            //尝试从空闲队列中获取一个连接，如果获取失败，则返回方法体首行
//            if (!FreeConnectionQueue.TryDequeue(out mqConnection)) //如果没有可用空闲连接，则重新进入等待排队
//            {
//                goto SelectMQConnectionLine;
//            }
//            else if (MQConnectionPoolUsingDicNew[mqConnection] + 1 > DefaultMaxConnectionUsingCount || !mqConnection.IsOpen) //如果取到空闲连接，判断是否使用次数是否超过最大限制,超过则释放连接并重新创建
//            {
//                mqConnection.Close();
//                mqConnection.Dispose();

//                mqConnection = CreateMQConnection();
//                MQConnectionPoolUsingDicNew[mqConnection] = 0;

//            }

//            BusyConnectionDic[mqConnection] = true;//加入到忙连接集合中
//            MQConnectionPoolUsingDicNew[mqConnection] = MQConnectionPoolUsingDicNew[mqConnection] + 1;//使用次数加1

//            return mqConnection;
//        }

//        /// <summary>
//        /// 将一个连接放回连接池中
//        /// </summary>
//        /// <param name="connection"></param>
//        private static void ResetMQConnectionToFree(IConnection connection)
//        {
//            lock (freeConnLock)
//            {
//                bool result = false;
//                if (BusyConnectionDic.TryRemove(connection, out result)) //从忙队列中取出
//                {
//                    //  BaseUtil.Logger.DebugFormat("set FreeConnectionQueue:{0},FreeConnectionCount:{1}, BusyConnectionCount:{2}", connection.GetHashCode().ToString(), FreeConnectionQueue.Count, BusyConnectionDic.Count);
//                }
//                else
//                {
//                    // BaseUtil.Logger.DebugFormat("failed TryRemove BusyConnectionDic:{0},FreeConnectionCount:{1}, BusyConnectionCount:{2}", connection.GetHashCode().ToString(), FreeConnectionQueue.Count, BusyConnectionDic.Count);
//                }

//                if (FreeConnectionQueue.Count + BusyConnectionDic.Count > MaxConnectionCount)//如果因为高并发出现极少概率的>MaxConnectionCount，则直接释放该连接
//                {
//                    connection.Close();
//                    connection.Dispose();
//                }
//                else
//                {
//                    FreeConnectionQueue.Enqueue(connection);//加入到空闲队列，以便持续提供连接服务
//                }

//                MQConnectionPoolSemaphore.Release();//释放一个空闲连接信号

//            }
//        }

//        /// <summary>
//        /// 发送消息
//        /// </summary>
//        /// <param name="connection">消息队列连接对象</param>
//        /// <typeparam name="T">消息类型</typeparam>
//        /// <param name="queueName">队列名称</param>
//        /// <param name="durable">是否持久化</param>
//        /// <param name="msg">消息</param>
//        /// <returns></returns>
//        public static string SendMsg(IConnection connection, string queueName, string msg, bool durable = true)
//        {
//            try
//            {

//                using (var channel = connection.CreateModel())//建立通讯信道
//                {
//                    // 参数从前面开始分别意思为：队列名称，是否持久化，独占的队列，不使用时是否自动删除，其他参数
//                    channel.QueueDeclare(queueName, durable, false, false, null);

//                    var properties = channel.CreateBasicProperties();
//                    properties.DeliveryMode = 2;//1表示不持久,2.表示持久化

//                    if (!durable)
//                        properties = null;

//                    var body = Encoding.UTF8.GetBytes(msg);
//                    channel.BasicPublish("", queueName, properties, body);
//                }


//                return string.Empty;
//            }
//            catch (Exception ex)
//            {
//                return ex.ToString();
//            }
//            finally
//            {
//                ResetMQConnectionToFree(connection);
//            }
//        }

//        /// <summary>
//        /// 消费消息
//        /// </summary>
//        /// <param name="connection">消息队列连接对象</param>
//        /// <param name="queueName">队列名称</param>
//        /// <param name="durable">是否持久化</param>
//        /// <param name="dealMessage">消息处理函数</param>
//        /// <param name="saveLog">保存日志方法，可选</param>
//        public static void ConsumeMsg(IConnection connection, string queueName, bool durable, Func<string, ConsumeAction> dealMessage, Action<string, Exception> saveLog = null)
//        {
//            try
//            {

//                using (var channel = connection.CreateModel())
//                {
//                    channel.QueueDeclare(queueName, durable, false, false, null); //获取队列 
//                    channel.BasicQos(0, 1, false); //分发机制为触发式

//                    var consumer = new QueueingBasicConsumer(channel); //建立消费者
//                    // 从左到右参数意思分别是：队列名称、是否读取消息后直接删除消息，消费者
//                    channel.BasicConsume(queueName, false, consumer);

//                    while (true)  //如果队列中有消息
//                    {
//                        ConsumeAction consumeResult = ConsumeAction.RETRY;
//                        var ea = consumer.Queue.Dequeue(); //获取消息
//                        string message = null;

//                        try
//                        {
//                            var body = ea.Body;
//                            message = Encoding.UTF8.GetString(body);
//                            consumeResult = dealMessage(message);
//                        }
//                        catch (Exception ex)
//                        {
//                            saveLog?.Invoke(message, ex);
//                        }
//                        if (consumeResult == ConsumeAction.ACCEPT)
//                        {
//                            channel.BasicAck(ea.DeliveryTag, false);  //消息从队列中删除
//                        }
//                        else if (consumeResult == ConsumeAction.RETRY)
//                        {
//                            channel.BasicNack(ea.DeliveryTag, false, true); //消息重回队列
//                        }
//                        else
//                        {
//                            channel.BasicNack(ea.DeliveryTag, false, false); //消息直接丢弃
//                        }
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                saveLog?.Invoke("QueueName:" + queueName, ex);

//                throw ex;
//            }
//            finally
//            {
//                ResetMQConnectionToFree(connection);
//            }
//        }

//        /// <summary>
//        /// 依次获取单个消息
//        /// </summary>
//        /// <param name="connection">消息队列连接对象</param>
//        /// <param name="QueueName">队列名称</param>
//        /// <param name="durable">持久化</param>
//        /// <param name="dealMessage">处理消息委托</param>
//        public static void ConsumeMsgSingle(IConnection connection, string QueueName, bool durable, Func<string, ConsumeAction> dealMessage)
//        {
//            try
//            {

//                using (var channel = connection.CreateModel())
//                {
//                    channel.QueueDeclare(QueueName, durable, false, false, null); //获取队列 
//                    channel.BasicQos(0, 1, false); //分发机制为触发式

//                    uint msgCount = channel.MessageCount(QueueName);

//                    if (msgCount > 0)
//                    {
//                        var consumer = new QueueingBasicConsumer(channel); //建立消费者
//                        // 从左到右参数意思分别是：队列名称、是否读取消息后直接删除消息，消费者
//                        channel.BasicConsume(QueueName, false, consumer);

//                        ConsumeAction consumeResult = ConsumeAction.RETRY;
//                        var ea = consumer.Queue.Dequeue(); //获取消息
//                        try
//                        {
//                            var body = ea.Body;
//                            var message = Encoding.UTF8.GetString(body);
//                            consumeResult = dealMessage(message);
//                        }
//                        catch (Exception ex)
//                        {
//                            throw ex;
//                        }
//                        finally
//                        {
//                            if (consumeResult == ConsumeAction.ACCEPT)
//                            {
//                                channel.BasicAck(ea.DeliveryTag, false);  //消息从队列中删除
//                            }
//                            else if (consumeResult == ConsumeAction.RETRY)
//                            {
//                                channel.BasicNack(ea.DeliveryTag, false, true); //消息重回队列
//                            }
//                            else
//                            {
//                                channel.BasicNack(ea.DeliveryTag, false, false); //消息直接丢弃
//                            }
//                        }
//                    }
//                    else
//                    {
//                        dealMessage(string.Empty);
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                ResetMQConnectionToFree(connection);
//            }
//        }

//        /// <summary>
//        /// 获取队列消息数
//        /// </summary>
//        /// <param name="connection"></param>
//        /// <param name="QueueName"></param>
//        /// <returns></returns>
//        public static int GetMessageCount(IConnection connection, string QueueName)
//        {
//            int msgCount = 0;
//            try
//            {

//                using (var channel = connection.CreateModel())
//                {
//                    channel.QueueDeclare(QueueName, true, false, false, null); //获取队列 
//                    msgCount = (int)channel.MessageCount(QueueName);
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                ResetMQConnectionToFree(connection);
//            }

//            return msgCount;
//        }


//    }

//    public enum ConsumeAction
//    {
//        ACCEPT,  // 消费成功
//        RETRY,   // 消费失败，可以放回队列重新消费
//        REJECT,  // 消费失败，直接丢弃
//    }
//}