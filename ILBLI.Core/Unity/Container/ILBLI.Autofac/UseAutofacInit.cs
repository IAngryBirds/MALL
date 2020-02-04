using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ILBLI.Unity
{
    /// <summary>
    /// 自定义注入Autofac
    /// </summary>
    public static class UseAutofacInit
    {
        /// <summary>
        /// 需要注入的Assembly程序集
        /// </summary>
        private static readonly List<string> _LibNameList = new List<string>() { "ILBLI.Repository", "ILBLI.Service" }; 

        /// <summary>
        /// core2.0下实现替换自己的Autofac
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceProvider AddAutofacInit(this IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            // 获取项目所有程序集，排除Microsft,Nuget下载的，并且程序集以ILBLI开头
            //var libs = DependencyContext.Default.CompileLibraries?.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Name.StartsWith("ILBLI."));

            List<Assembly> assemblys = new List<Assembly>();
            foreach (var libName in _LibNameList)
            {
                assemblys.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(libName)));
            }

            if (assemblys.Count>0)
            {
                //将接口注入仓储的所有接口进行注入//过滤：留下继承了IBaseRepository/IService接口的类
                //AsImplementedInterfaces()让具体实现类型，可以该类型继承的所有接口类型找到该实现类型
                builder.RegisterAssemblyTypes(assemblys.ToArray())
                    .Where(x => typeof(IRepository).IsAssignableFrom(x) || typeof(IService).IsAssignableFrom(x))
                    .AsImplementedInterfaces();
            }
             
            //将core默认的IOC容器替换成自己的
            builder.Populate(services);
             
            var container = builder.Build();
            
            return new AutofacServiceProvider(container);
        }

        /// <summary>
        /// core 3.0下替换自己的Autofac
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder AddAutofacInit(this ContainerBuilder builder)
        {
            // 获取项目所有程序集，排除Microsft,Nuget下载的，并且程序集以ILBLI开头
            //var libs = DependencyContext.Default.CompileLibraries?.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Name.StartsWith("ILBLI."));

            List<Assembly> assemblys = new List<Assembly>();
            foreach (var libName in _LibNameList)
            {
                assemblys.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(libName)));
            }


            if (assemblys.Count > 0)
            {
                //将接口注入仓储的所有接口进行注入//过滤：留下继承了IBaseRepository/IService接口的类
                //AsImplementedInterfaces()让具体实现类型，可以该类型继承的所有接口类型找到该实现类型
                builder.RegisterAssemblyTypes(assemblys.ToArray())
                    .Where(x => typeof(IRepository).IsAssignableFrom(x) || typeof(IService).IsAssignableFrom(x))
                    .AsImplementedInterfaces();
            }

            return builder;
        }

    }
}
