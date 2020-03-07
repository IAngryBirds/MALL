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
            #region �Զ������֤��ϵ  

            /*services.AddAuthenticationCore(option =>
            {
                option.RequireAuthenticatedSignIn = false;
                option.AddScheme<ILBLISchemeHandler>(typeof(ILBLISchemeHandler).Name, "ilbliScheme");
                option.DefaultScheme = typeof(ILBLISchemeHandler).Name;

            });*/

            #endregion

            //ʹ��Jwt����֤ģʽ
            services.AddJwtAuthentication("ilbli_custom_key");

            //�����еĿ������඼����DIע�룬Ҫʹ�������
            services.AddControllers();

            //�Լ���װ��swagger��ʼ��ע��
            services.AddSwaggerInit();

            //����ȫ�ֿ���
            services.AddCors(cros => {   });

            //�����Զ����AutoMapper
            services.AddAutoMapperInit();

            //������������
            services.AddFilterInit();

            //����SqlSuager��ʼ������
            services.AddSqlSurgar(this._Configuration);

            //���ע��ѩ���㷨����Ψһ��UID
            services.AddSnowFlak();

            //��ӻ�����Redis�������
            services.AddBaseRedisCache(this._Configuration);
            //�����չ��Redis�������
            services.AddRedisServiceCache(this._Configuration);
            //���RabbitMQ����
            services.AddRabbitMQService(this._Configuration);
        }

        //�ص㣺Ҫע���м��ע���˳��UseStaticFiles --��UseRouting --��UseCors --����Ȩ�����м���� --�� UseEndpoints��
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            //����Լ���װ��swaggerUI
            app.UseSwaggerUIInit();
            
            app.UseRouting();
            
            //��������
            app.UseCors();

            //����֤����Ȩ
            app.UseAuthenticationThenAuthorization();

            //�˵�·��--������ʹ��app.UseRouting()��������ConfigureServices �� 
            //�����еĿ������඼����DIע��services.AddControllers();
            app.UseEndpoints(endpoints =>
            { 
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"); 
            });

        }

        /// <summary>
        /// ���ConfigureContainer ������ʹ��ConfigureContainer����Autofac��������������ֱ����Autofacע��
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddAutofacInit();
        }
    }
}
