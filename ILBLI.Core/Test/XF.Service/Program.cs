using ILBLI.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XF.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            JsonMQConfig config = new JsonMQConfig()
            {
                HostServer = new HostServer
                {
                    HostName = new List<string> { "localhost" },
                    UserName = "ilbli",
                    Password = "123456",
                    VirtualHost="/",
                    Port = 15672
                },
                Queues = new List<ExchangeQueue>
                {
                    new ExchangeQueue {
                        AutoDelete = false,
                        Durable = true,
                        ExchangeName = "ILBLI.Exchange",
                        ExchangeType = "direct",
                        QueueKey = "OATest",
                        QueueName = "OATest",
                        RoutingKey = "OATest",
                        PrefetchCount=300
                    }
                }
            };
            
            RabbitManage rabbit = new RabbitManage(config);
            IConnection conn = null;
            while (true)
            { 
                Task.Run(() =>
                {
                    conn = rabbit.ConsumeReturnConnection("OATest", x =>
                    {
                        Console.WriteLine(x);
                    });
                });
                #region 关闭链接

                Console.WriteLine("输入Quit关闭连接");
                var input = "";
                while ((input = Console.ReadLine()) != "Quit")
                {
                    Console.WriteLine("无效的输入");
                }

                #endregion

                #region 关闭连接释放资源

                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                } 

                #endregion

                #region 重启服务

                Console.WriteLine("Rabbit消费者已断开连接，如需重新启动，请输入Start...");
                input = "";
                while ((input = Console.ReadLine()) != "Start")
                {
                    Console.WriteLine("无效的输入");
                }

                #endregion
            }

            Console.ReadKey();
        }
    }
}
