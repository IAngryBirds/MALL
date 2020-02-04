using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SqlSugar;
namespace ILBLI.SqlSugar
{
    public static class AddSqlSugarInit
    {
        /// <summary>
        /// 注册数据库ORM框架服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置信息</param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSurgar(this IServiceCollection services, IConfiguration configuration)
        {
            /*
             * 将Appsetting.json的配置信息通过实体类的实现注入容器，使得可以通过 
             *   private readonly ConnectionConfig _Config; 
             *   protected DBRepository(IOptions<ConnectionConfig> config) {  _Config = config.Value; }
             */
            //services.AddOptions();  
            services.Configure<ConnectionConfig>(configuration.GetSection(typeof(ConnectionConfig).Name));
            return services;
        }
    }
}
