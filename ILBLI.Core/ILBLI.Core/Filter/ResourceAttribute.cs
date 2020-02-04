using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace ILBLI.Unity
{
    /// <summary>
    /// 第二层（Conntroller实例化之前）
    /// </summary>
    public class ResourceAttribute : Attribute, IResourceFilter
    {
        private readonly ILogger<ResourceAttribute> _Logger;

        public ResourceAttribute(ILogger<ResourceAttribute> logger)
        {
            this._Logger = logger;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _Logger.LogWarning("Result_第二层资源拦截器--Controller实例化之前");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _Logger.LogWarning("第二层资源拦截器--Controller实例化之前"); 
        }
    }
}
