using System.Collections.Generic;

namespace ILBLI.RabbitMQ
{
    public class HostServer
    {
        /// <summary>
        /// MQ服务器列表
        /// </summary>
        public List<string> HostName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 虚拟机名
        /// </summary>
        public string VirtualHost { get; set; } = "/";

        /// <summary>
        /// 自否自动重连
        /// </summary>
        public bool AutomaticRecoveryEnabled { get; set; } = true;

        /// <summary>
        /// 心跳超时时间
        /// 如果你连接单台节点的时候不设置这个值是没问题的，但是如果你连接的是类似HAProxy虚拟节点的时候就会出现TCP被断开的可能性。如果你不设置这个心跳超时时间，它默认是不进行心跳保持的，就会出现网络中的某个设置断开空闲的TCP连接资源
        /// </summary>
        public ushort RequestedHeartbeat { get; set; } = 60;
    }
}
