using RabbitMQ.Client;
using System;

namespace ILBLI.RabbitMQ
{
    public interface IRabbitManage
    {
        /// <summary>
        /// 返回一个MQ链接
        /// </summary>
        /// <returns></returns>
        IConnection GetConnection();
         
        /// <summary>
        /// 创建一个发送消息的MQ信道
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        IModel CreateSendChannel(ExchangeQueue queue);

        /// <summary>
        /// 发送MQ消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueKey">队列任务的唯一ID(需要与json配置文件统一)</param>
        /// <param name="message">需要入队列的消息</param>
        void SendToMQ<T>(string queueKey, T message);
        
        /// <summary>
        /// 消费队列
        /// </summary>
        /// <param name="queueKey">队列任务的唯一ID(需要与json配置文件统一)</param>
        /// <param name="action">后续处理委托</param>
        void ConsumeGenerator(string queueKey, Action<string> action);

        /// <summary>
        /// 消费队列
        /// </summary>
        /// <param name="queueKey">队列任务的唯一ID(需要与json配置文件统一)</param>
        /// <param name="action">后续处理委托</param>
        IConnection ConsumeReturnConnection(string queueKey, Action<string> action);

    }
}
