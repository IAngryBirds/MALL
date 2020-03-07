using ILBLI.RabbitMQ;
using System;
using System.Collections.Generic;

namespace SC.Service
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
                        RoutingKey = "OATest"
                    }
                }
            };

            RabbitManage rabbit = new RabbitManage(config);
            //rabbit.SendToMQ("OATest", "这是新的通告：2012您好");
            Console.WriteLine("请输入需要推送的消息... 'Quit' to quit.");

            var input = ""; 
            while ((input = Console.ReadLine()) != "Quit")
            {
                rabbit.SendToMQ("OATest", input);
            }
             
            Console.ReadKey();
        }
    }
}
