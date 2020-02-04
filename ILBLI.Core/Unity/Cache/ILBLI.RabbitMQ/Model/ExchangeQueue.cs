namespace ILBLI.RabbitMQ 
{
    /// <summary>
    /// 交换器--队列信息
    /// </summary>
    public class ExchangeQueue
    {
        /// <summary>
        /// 队列唯一表示符
        /// </summary>
        public string QueueKey { get; set; }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 交换机类型[direct分发Exchange下所有绑定了同一个routingKey队列;fanout：分发Exchange下所有队列;topic通配符匹配;headers不常用]
        /// </summary>
        public string ExchangeType { get; set; }

        /// <summary>
        /// 队列名
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// 是否持久化[默认支持持久化]
        /// </summary>
        public bool Durable { get; set; } = true;

        /// <summary>
        /// 是否允许自动删除[默认不删除]
        /// </summary>
        public bool AutoDelete { get; set; } = false;

        /// <summary>
        /// 如果采用手动应答BasicAck模式，那么当未应答的数量达到n条时，就不会再往该消费者进行消息推送
        /// 【即为了防止消费者出现故障，一直无法应答或处理不过来时，消息还一直往这个消费者队列推送，导致服务不可用，
        /// 这样设置后最多有异常的数据就N条】【推送方式还是通过轮训一条条的推送给消费者，而不是理解中的一下子推送N条数据的概念】
        /// </summary>
        public ushort PrefetchCount { get; set; } = 1;
    }
}
