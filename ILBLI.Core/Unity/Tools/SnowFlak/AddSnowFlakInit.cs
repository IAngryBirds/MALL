using Microsoft.Extensions.DependencyInjection;

namespace ILBLI.SnowFlak
{
    public static class AddSnowFlakInit
    {
        /// <summary>
        /// 添加雪花算法获取UID
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSnowFlak(this IServiceCollection services)
        {
            services.AddSingleton<ISnowFlak, SnowFlakManager>();
            return services;
        }
    }
}
