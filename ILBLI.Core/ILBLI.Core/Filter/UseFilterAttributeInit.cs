using Microsoft.Extensions.DependencyInjection;

namespace ILBLI.Unity
{
    public static class UseFilterAttributeInit
    {
        public static IServiceCollection AddFilterInit(this IServiceCollection services)
        {
            //注册拦截器层
            services.AddMvcCore(option => {
                option.Filters.Add<ExceptionAttribute>();
                //拦截器顺序 IAuthorizationFilter --> IResourceFilter -->IActionFilter --> IResultFilter 跟注入时的顺序无关
                //日志打印顺序: IAuthorizationFilter_In --> IResourceFilter_In --> IActionFilter_In -->[方法体] -->
                //              IActionFilter_Out --> IResultFilter_In -->IResultFilter_Out -->IResourceFilter_Out  
                option.Filters.Add<ResultAttribute>();
                option.Filters.Add<DataAnnotationAttribute>();
                option.Filters.Add<ResourceAttribute>();
                option.Filters.Add<AuthorizationAttribute>(); 
            });
            return services;
        }

    }
}
