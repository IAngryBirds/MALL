using System;
using System.Collections.Generic;

namespace ILBLI.Unity
{
    public class TResult
    { 
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Status { get; set; } = false;

        /// <summary>
        /// 处理结果代码
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 处理结果信息
        /// </summary>
        public string Message { get; set; } = "请求成功";

        /// <summary>
        /// 返回的数据信息
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 返回默认的成功结果
        /// </summary>
        /// <returns></returns>
        public static TResult Success()
        {
            return new TResult
            {
                Status = true
            };
        }
        
        /// <summary>
        /// 返回默认的失败结果
        /// </summary>
        /// <returns></returns>
        public static TResult Fail(string message="")
        {
            return new TResult
            {
                Status = false,
                Message = string.IsNullOrWhiteSpace(message) ? "请求失败" : message
            };
        }

        /// <summary>
        /// 返回查询实体信息的成功结果
        /// </summary>
        /// <typeparam name="TModel">实体类型</typeparam>
        /// <param name="model">实体信息</param>
        /// <returns></returns>
        public static TResult DataSuccess(object model)
        {
            return new TResult
            {
                Status = true, 
                Data = model
            };
        }

        /// <summary>
        /// 返回查询实体集合的成功结果
        /// </summary>
        /// <typeparam name="TModel">实体类型</typeparam>
        /// <param name="list">实体集合信息</param>
        /// <returns></returns>
        public static TResult ListSuccess<T>(List<T> list)
        {
            return new TResult
            {
                Status = true,
                Data = list
            };
        }

        /// <summary>
        /// 返回接口交互信息
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns></returns>
        public static TResult TupleEntity(Tuple<bool, string,object> tuple)
        {
            return new TResult
            {
                Status = tuple.Item1,
                Message = tuple.Item2,
                Data = tuple.Item3
            };
        }
         
    }
}
