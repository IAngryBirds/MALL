using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILBLI.RabbitMQ
{
    public class RabbitManage:IRabbitManage
    {

        //单例设计，获取一次连接【一个任务队列】
        private ConcurrentDictionary<string, IModel> ChannelSendDictionary = new ConcurrentDictionary<string, IModel>();

        private readonly HostServer _HostServer;
        private readonly List<ExchangeQueue> _Queues;
        private readonly ConnectionFactory _RabbitMqFactory;
        public RabbitManage(JsonMQConfig mQConfig)
        {
            this._HostServer = mQConfig.HostServer;
            this._Queues = mQConfig.Queues;
            //连接参数配置
            if(_HostServer==null || _Queues == null)
            {
                throw new ArgumentNullException("RabbitMQ的配置信息初始化异常：配置信息缺失...");
            }

            //服务器连接配置
            this._RabbitMqFactory = new ConnectionFactory
            {
                HostName = _HostServer.HostName.FirstOrDefault(),
                UserName = _HostServer.UserName,
                Password = _HostServer.Password,
                VirtualHost = _HostServer.VirtualHost,
                RequestedHeartbeat = _HostServer.RequestedHeartbeat,
                AutomaticRecoveryEnabled = _HostServer.AutomaticRecoveryEnabled
            }; 
        }

        /// <summary>
        /// 返回一个MQ链接
        /// </summary>
        /// <returns></returns>
        public IConnection GetConnection()
        {
            return _RabbitMqFactory.CreateConnection();
        }

        /// <summary>
        /// 创建一个发送消息的MQ信道
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public IModel CreateSendChannel(ExchangeQueue queue)
        {
            //客户端连接到消息队列服务器【TCP】
            var conn = _RabbitMqFactory.CreateConnection();
            //打开一个channel信道
            var channel = conn.CreateModel();
            //客户端声明一个Exchange,并设置相关属性
            channel.ExchangeDeclare(exchange: queue.ExchangeName, type: queue.ExchangeType, durable: queue.Durable, autoDelete: queue.AutoDelete, arguments: null);
            //客户端声明一个Queue，并设置相关属性
            channel.QueueDeclare(queue: queue.QueueName, durable: queue.Durable, autoDelete: queue.AutoDelete, exclusive: false, arguments: null);
            //客户端使用Routing Key ,在Exchange和Queue之间建立好绑定关系 
            channel.QueueBind(queue: queue.QueueName, exchange: queue.ExchangeName, routingKey: queue.RoutingKey);

            return channel;
        }

        /// <summary>
        /// 发送消息队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueKey"></param>
        /// <param name="message"></param>
        public void SendToMQ<T>(string queueKey,T message)
        {
            ExchangeQueue queue = _Queues.Find(x => x.QueueKey.Equals(queueKey, StringComparison.OrdinalIgnoreCase));
             
            IModel channel = ChannelSendDictionary.GetOrAdd(queueKey, (queueKey) =>
            {
                return CreateSendChannel(queue);
            });
            if (queue == null)
            {
                throw new KeyNotFoundException("当前队列配置信息不存在...");
            }
            if (channel==null)
            {
                throw new KeyNotFoundException("当前队列ID不存在...");
            }
            
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            string jsonStr = JsonConvert.SerializeObject(message);
            byte[] bodys = Encoding.UTF8.GetBytes(jsonStr);

            //客户端投递消息到Exchange
            channel.BasicPublish(queue.ExchangeName, queue.RoutingKey, properties, bodys);
        }

        /// <summary>
        /// 消费队列
        /// </summary>
        /// <param name="queueKey"></param>
        /// <param name="action"></param>
        public void ConsumeGenerator(string queueKey,Action<string> action)
        {
            ExchangeQueue queue = _Queues.Find(x => x.QueueKey.Equals(queueKey, StringComparison.OrdinalIgnoreCase));
             
            if (queue == null)
            {
                throw new KeyNotFoundException("当前队列配置信息不存在...");
            }

            #region 新打开一个消费的队列信道

            //客户端连接到消息队列服务器【TCP】
            var conn = _RabbitMqFactory.CreateConnection();
            //打开一个channel信道
            var channel = conn.CreateModel();
            //客户端声明一个Exchange,并设置相关属性[消费者不需要指定Exchange]
            //channel.ExchangeDeclare(exchange: queue.ExchangeName, type: queue.ExchangeType, durable: queue.Durable, autoDelete: queue.AutoDelete, arguments: null);
            //客户端声明一个Queue，并设置相关属性
            channel.QueueDeclare(queue: queue.QueueName, durable: queue.Durable, autoDelete: queue.AutoDelete, exclusive: false, arguments: null);
            //客户端使用Routing Key ,在Exchange和Queue之间建立好绑定关系 [消费者不需要绑定]
            // channel.QueueBind(queue: queue.QueueName, exchange: queue.ExchangeName, routingKey: queue.RoutingKey);

            #endregion
            #region 消费消息数据
            // 设置prefetchCount : 1来告知RabbitMQ，在未收到消费端的消息确认时，不再分发消息，也就确保了当消费端处于忙碌状态时，不继续发送数据
            channel.BasicQos(prefetchSize: 0, prefetchCount: queue.PrefetchCount, global: false);
            //设置消息持久化【队列持久化，Exchange持久化，在设置消息持久化，三个同时设置，才能做到消息的真正持久化】
            //定义这个队列的消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            //false为手动应答，true为自动应答
            channel.BasicConsume(queue.QueueName, false, consumer);

            #endregion

            //客户端投递消息到Exchange
            consumer.Received += (model, ea) => {
                var msgBody = Encoding.UTF8.GetString(ea.Body);

                //调用后续处理的委托
                action.Invoke(msgBody); 

                //确认该消息已被消费
                channel.BasicAck(ea.DeliveryTag, false); 
            }; 
        }

        /// <summary>
        /// 消费队列
        /// </summary>
        /// <param name="queueKey"></param>
        /// <param name="action"></param>
        public IConnection ConsumeReturnConnection(string queueKey, Action<string> action)
        {
            ExchangeQueue queue = _Queues.Find(x => x.QueueKey.Equals(queueKey, StringComparison.OrdinalIgnoreCase));

            if (queue == null)
            {
                throw new KeyNotFoundException("当前队列配置信息不存在...");
            }

            #region 新打开一个消费的队列信道

            //客户端连接到消息队列服务器【TCP】
            var conn = _RabbitMqFactory.CreateConnection();
            //打开一个channel信道
            var channel = conn.CreateModel();
            //客户端声明一个Exchange,并设置相关属性[消费者不需要指定Exchange]
            //channel.ExchangeDeclare(exchange: queue.ExchangeName, type: queue.ExchangeType, durable: queue.Durable, autoDelete: queue.AutoDelete, arguments: null);
            //客户端声明一个Queue，并设置相关属性
            channel.QueueDeclare(queue: queue.QueueName, durable: queue.Durable, autoDelete: queue.AutoDelete, exclusive: false, arguments: null);
            //客户端使用Routing Key ,在Exchange和Queue之间建立好绑定关系 [消费者不需要绑定]
            // channel.QueueBind(queue: queue.QueueName, exchange: queue.ExchangeName, routingKey: queue.RoutingKey);

            #endregion
            #region 消费消息数据
            // 设置prefetchCount : 1来告知RabbitMQ，在未收到消费端的消息确认时，不再分发消息，也就确保了当消费端处于忙碌状态时，不继续发送数据
            channel.BasicQos(prefetchSize: 0, prefetchCount: queue.PrefetchCount, global: false);
            //设置消息持久化【队列持久化，Exchange持久化，在设置消息持久化，三个同时设置，才能做到消息的真正持久化】
            //定义这个队列的消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            //false为手动应答，true为自动应答
            channel.BasicConsume(queue.QueueName, false, consumer);

            #endregion

            //客户端投递消息到Exchange
            consumer.Received += (model, ea) => {
                var msgBody = Encoding.UTF8.GetString(ea.Body);

                //调用后续处理的委托
                action.Invoke(msgBody); 
                //确认该消息已被消费
                channel.BasicAck(ea.DeliveryTag, false);
            };

            return conn;
        }
    }
}
