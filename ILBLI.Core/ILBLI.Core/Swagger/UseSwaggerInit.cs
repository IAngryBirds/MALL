using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;

namespace ILBLI.Unity.Swagger
{
    public static class UseSwaggerInit
    {
        /// <summary>
        /// 中间件注册
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerUIInit(this IApplicationBuilder app)
        {
            //启用中间件服务生成swagger作为json的终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，制定Swagger Json终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); 
            });

            return app;
        }

        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerInit(this IServiceCollection services,List<string> xmlPath=null)
        {
            //注册Swagger生成器，定义一个和多个Swagger文档
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MY API",
                    Description = "ASP.NET Core Web API",
                    TermsOfService = new Uri("https://ilbli.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "ilbli",
                        Email = "757102006@qq.com",
                        Url = new Uri("https://ilbli.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "许可证名字",
                        Url = new Uri("https://ilbli.com")
                    }
                }); ;
               
                if (xmlPath!=null && xmlPath.Count > 0)
                { 
                    // 为 Swagger JSON and UI设置xml文档注释路径
                    var basePath = Path.GetDirectoryName(typeof(UseSwaggerInit).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                    xmlPath.ForEach(p =>
                    {
                        c.IncludeXmlComments( Path.Combine(basePath,p));
                    });
                }

                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

            });



            return services;
        }

    }
}
