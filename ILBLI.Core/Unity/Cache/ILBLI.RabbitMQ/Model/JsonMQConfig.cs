using System.Collections.Generic;

namespace ILBLI.RabbitMQ
{
    /// <summary>
    /// Json配置文件
    /// </summary>
    public class JsonMQConfig
    {
        /// <summary>
        /// MQ服务器配置
        /// </summary>
        public HostServer HostServer { get; set; }
        /// <summary>
        /// 消息队列配置
        /// </summary>
        public List<ExchangeQueue> Queues { get; set; }
    }
}
