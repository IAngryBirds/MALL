using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace ILBLI.Unity
{
    /// <summary>
    /// 第三层（Conntroller实例化之后）
    /// </summary>
    public class DataAnnotationAttribute : Attribute, IActionFilter
    {
        private readonly ILogger<DataAnnotationAttribute> _Logger;

        public DataAnnotationAttribute(ILogger<DataAnnotationAttribute> logger)
        {
            this._Logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _Logger.LogWarning("Result_第三层实体类拦截器--Controller实例化之后");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _Logger.LogWarning("第三层实体类拦截器--Controller实例化之后");
        }
    }
}
