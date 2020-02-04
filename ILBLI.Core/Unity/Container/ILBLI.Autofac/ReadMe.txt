1.需要引用：Autofac |Autofac.Extensions.DependencyInjection |Autofac.Extras.DynamicProxy
2.(Core2.0实现方法)需要改写WebAPI--Startup--ConfigureServices方法： 将原来的void 改成 返回类型为IServiceProvider，并且再最后加上 return services.AddAutofacInit();
3.(Core3.0实现方法)需要改写WebAPI--Program--CreateHostBuilder方法： 在Host.CreateDefaultBuilder(args)下添加.UseServiceProviderFactory(new AutofacServiceProviderFactory()) ，并且在Startup--添加方法
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddAutofacInit();
        }