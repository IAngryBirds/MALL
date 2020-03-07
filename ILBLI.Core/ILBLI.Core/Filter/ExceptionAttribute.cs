using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace ILBLI.Unity
{
    /// <summary>
    /// 第四层（Conntroller实例化之后）
    /// </summary>
    public class ExceptionAttribute : Attribute, IExceptionFilter
    {
        private readonly ILogger<ExceptionAttribute> _Logger;

        public ExceptionAttribute(ILogger<ExceptionAttribute> logger)
        {
            _Logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            //如果截获异常为我们自定义，可以处理的异常则通过我们自己的规则处理
            if (context.Exception is DemoException)
            {
                //TODO:记录日志 
                _Logger.LogError(context.Exception,"自定义异常处理");
                context.Result = new BadRequest(context.Exception.Message); 
            }
            else
            {
                //如果截获异常是我没无法预料的异常，则将通用的返回信息返回给用户，避免泄露过多信息，也便于用户处理 
                //TODO:记录日志
                _Logger.LogError(context.Exception, "意料之外的异常处理");
                context.Result = new BadRequest(new { Message = "服务器被外星人拐跑了！" });

            }
        }
    }


    /// <summary>
    /// 请求失败
    /// </summary>
    public class BadRequest : ObjectResult
    {
        public BadRequest(object value):base(value)
        {
            StatusCode = 400;
        }
    }

}
