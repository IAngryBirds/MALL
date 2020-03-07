using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace ILBLI.RabbitMQ
{
    public static class AddRabbitMQInit
    {

        /// <summary>
        /// 添加RabbitMQ服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMQService(this IServiceCollection services, IConfiguration configuration)
        {
            /*读取配置文件信息【通过Bind方法，对配置文件与实体进行绑定】*/
            JsonMQConfig config = new JsonMQConfig();
            configuration.Bind(nameof(JsonMQConfig),config);  
             

            if (config == null)
            {
                throw new ArgumentNullException("MQRibbit初始化失败：配置信息缺失...");
            }
            //注入单例--RabbitManage [使用接口代替实例]
            services.AddSingleton<IRabbitManage>(new RabbitManage(config));
      
            return services;
        }
    }
}
