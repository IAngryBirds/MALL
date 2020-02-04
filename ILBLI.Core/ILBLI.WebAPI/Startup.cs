using Autofac;
using ILBLI.RabbitMQ;
using ILBLI.Redis;
using ILBLI.SnowFlak;
using ILBLI.SqlSugar;
using ILBLI.Unity;
using ILBLI.Unity.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ILBLI.WebAPI
{
    public class Startup
    {
        private readonly IConfiguration _Configuration;
        public Startup(IConfiguration configuration)
        {
            this._Configuration = configuration; 
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region 自定义的认证体系  

            /*services.AddAuthenticationCore(option =>
            {
                option.RequireAuthenticatedSignIn = false;
                option.AddScheme<ILBLISchemeHandler>(typeof(ILBLISchemeHandler).Name, "ilbliScheme");
                option.DefaultScheme = typeof(ILBLISchemeHandler).Name;

            });*/

            #endregion

            //使用Jwt的认证模式
            services.AddJwtAuthentication("ilbli_custom_key");

            //将所有的控制器类都进行DI注入，要使用下面的
            services.AddControllers();

            //自己封装的swagger初始化注入
            services.AddSwaggerInit();

            //配置全局跨域
            services.AddCors(cros => {   });

            //配置自定义的AutoMapper
            services.AddAutoMapperInit();

            //配置拦截器层
            services.AddFilterInit();

            //配置SqlSuager初始化数据
            services.AddSqlSurgar(this._Configuration);

            //添加注入雪花算法生成唯一的UID
            services.AddSnowFlak();

            //添加基础的Redis缓存操作
            services.AddBaseRedisCache(this._Configuration);
            //添加扩展的Redis缓存操作
            services.AddRedisServiceCache(this._Configuration);
            //添加RabbitMQ服务
            services.AddRabbitMQService(this._Configuration);
        }

        //重点：要注意中间件注册的顺序【UseStaticFiles --》UseRouting --》UseCors --》【权限类中间件】 --》 UseEndpoints】
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            //添加自己封装的swaggerUI
            app.UseSwaggerUIInit();
            
            app.UseRouting();
            
            //跨域设置
            app.UseCors();

            //先认证再授权
            app.UseAuthenticationThenAuthorization();

            //端点路由--必须先使用app.UseRouting()，并且在ConfigureServices 中 
            //将所有的控制器类都进行DI注入services.AddControllers();
            app.UseEndpoints(endpoints =>
            { 
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"); 
            });

        }

        /// <summary>
        /// 添加ConfigureContainer 方法，使用ConfigureContainer访问Autofac容器生成器，并直接向Autofac注册
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddAutofacInit();
        }
    }
}
