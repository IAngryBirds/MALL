using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ILBLI.Redis
{
    public static class AddRedisInit
    {
        /// <summary>
        /// 添加基础的Redis缓存操作
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddBaseRedisCache(this IServiceCollection services, IConfiguration configuration) 
        { 
            /*读取配置文件信息(方式一)*/
            string instanceNameStr = configuration.GetSection($"{nameof(BaseCache)}:InstanceName").Value;
            string confStr = configuration.GetSection($"{nameof(BaseCache)}:Configuration").Value;

            //注入日志中的JsonCacheConfig配置（读取配置文件方式二）
            //用于实现自定义缓存的配置读取 BaseCache --> JsonCacheConfig _JsonCacheConfig
            services.Configure<JsonCacheConfig>(configuration.GetSection(nameof(JsonCacheConfig)));
            
            //注入基础的Cache
            services.AddDistributedRedisCache(option => {
                option.InstanceName = instanceNameStr;
                option.Configuration = confStr;
            });
            //注入封装的Cache使用工具类
            services.AddTransient<IBaseCache, BaseCache>();

            return services;
        }


        /// <summary>
        /// 添加扩展的Redis缓存操作
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisServiceCache(this IServiceCollection services, IConfiguration configuration)
        {
            /*读取配置文件信息*/ 
            string confStr = configuration.GetSection($"{nameof(BaseCache)}:Configuration").Value;

            //注入封装的CacheService使用工具类--使用实例的方式进行单例注册【可以单独注册某几个】【
            //注入接收实例的话也需要使用对应的实例类，因为没有对应的接口】
            services.AddSingleton(new RedisBaseService(confStr));
            services.AddSingleton(new RedisHashService(confStr));
            services.AddSingleton(new RedisListService(confStr));
            services.AddSingleton(new RedisSetService(confStr));
            services.AddSingleton(new RedisSortedSetService(confStr));
            services.AddSingleton(new RedisStringService(confStr));
            services.AddSingleton(new RedisSubService(confStr));
            return services;
        }
    }
}
