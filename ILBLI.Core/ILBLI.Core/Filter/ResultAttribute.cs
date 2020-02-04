using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace ILBLI.Unity
{
    /// <summary>
    /// 第五层（Conntroller实例化之后）
    /// </summary>
    public class ResultAttribute : Attribute, IResultFilter
    {
        private readonly ILogger<ResultAttribute> _Logger;

        public ResultAttribute(ILogger<ResultAttribute> logger)
        {
            this._Logger = logger;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            _Logger.LogWarning("Result_第五层资源拦截器--Controller实例化之后");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            _Logger.LogWarning("第五层资源拦截器--Controller实例化之后");
        }
    }
}
