using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ILBLI.Core
{
    /// <summary>
    /// 数据库实体类字段映射类
    /// </summary>
    public class DBPropertyHelper
    {
        /// <summary>
        /// 根据传入的实体类，自动映射返回需要操作的SQL语句字段内容
        /// </summary>
        /// <typeparam name="TModel">实体类</typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfo<TModel>()
        {
            try
            {
                IEnumerable<PropertyInfo> lists = typeof(TModel).GetProperties().ToList();

                lists = lists?.Where(x =>
                         x.GetCustomAttributes(typeof(IgnoreAttribute), true).Length != 1 // 排除IEnumerable类型的属性（一般此类属性是关联查询使用的）
                      );
               
                return lists;
            }
            catch (Exception e)
            {
                throw new Exception($"获取实体类的反射属性失败：{e.Message}");
            }
        }

        /// <summary>
        /// 根据传入的实体类，自动映射返回需要操作的SQL语句字段内容
        /// </summary>
        /// <typeparam name="TModel">实体类</typeparam>
        /// <param name="model">实体类数据值</param>
        /// <param name="isFitter">是否需要过滤字段 false ：否 / true：是</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfo<TModel>(TModel model,bool isFitter=false)
        {
            try
            {
                IEnumerable<PropertyInfo> lists = typeof(TModel).GetProperties().ToList();

                lists= lists?.Where(x =>
                        x.GetCustomAttributes(typeof(IgnoreAttribute), true).Length != 1 // 排除IEnumerable类型的属性（一般此类属性是关联查询使用的）
                      );
                if (isFitter)
                {
                    lists = lists.Where(
                            x=> x.GetValue(model)!=null //过滤掉字段值为NULL的数据
                            && Convert.ToString(x.GetValue(model))!="0001/1/1 0:00:00" //过滤掉字段值为初始时间格式的数据
                        );
                }
                return lists;
            }catch(Exception e)
            {
                throw new Exception($"获取实体类的反射属性失败：{e.Message}");
            }
        }

    }
}
