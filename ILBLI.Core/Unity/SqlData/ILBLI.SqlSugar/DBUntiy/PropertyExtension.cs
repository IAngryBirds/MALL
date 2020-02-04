using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ILBLI.SqlSugar
{
    /// <summary>
    /// 数据库实体类字段映射扩展类
    /// </summary>
    public static class PropertyExtension
    {

        /// <summary>
        /// 根据传入的实体类，自动映射返回需要操作的SQL语句字段内容
        /// </summary>
        /// <typeparam name="TModel">实体类</typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyIgnore<TModel>() where TModel :class, new()
        {
            try
            {
                IEnumerable<PropertyInfo> lists = typeof(TModel).GetProperties().ToList();
                //排除Ignore标记的字段
                lists = lists?.Where(x =>
                         !x.IsDefined(typeof(IgnoreAttribute), true)
                      );

                return lists;
            }
            catch (Exception ex)
            {
                throw new FormatException($"获取{typeof(TModel).Name}实体类的反射属性失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 根据传入的实体类，自动映射返回需要操作的SQL语句字段内容
        /// </summary>
        /// <typeparam name="TModel">实体类</typeparam>
        /// <param name="model">实体类数据值</param>
        /// <param name="isFitter">是否需要过滤(NULL与不符合的日期)字段 false ：否 / true：是</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyIgnoreFitter<TModel>(TModel model, bool ignoreFitter = false) where TModel : class, new()
        {
            try
            {
                IEnumerable<PropertyInfo> lists = typeof(TModel).GetProperties().ToList();
                 
                lists = lists?.Where(x =>
                         !x.IsDefined(typeof(IgnoreAttribute), true) 
                      );
                if (ignoreFitter)
                {
                    //过滤掉字段值为NULL的数据 && 过滤掉字段值为初始时间格式的数据
                    lists = lists.Where(
                            x => x.GetValue(model) != null 
                            && Convert.ToString(x.GetValue(model)) != "0001/1/1 0:00:00" 
                        );
                }
                return lists;
            }
            catch (Exception ex)
            {
                throw new FormatException($"获取{typeof(TModel).Name}实体类的反射属性失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取添加了指定的特性的字段
        /// </summary>
        /// <typeparam name="TModel">实体类</typeparam>
        /// <typeparam name="TAttribute">要进行查找的特性</typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyByAttribute<TModel, TAttribute>() 
            where TAttribute : Attribute 
            where TModel : class, new()
        {
            try
            {
                IEnumerable<PropertyInfo> lists = typeof(TModel).GetProperties().ToList();
                //查询所有字段属性中实现了TAttribute特性的字段内容
                lists = lists?.Where(x =>
                         x.IsDefined(typeof(TAttribute), true)
                      );

                return lists;
            }
            catch (Exception ex)
            {
                throw new FormatException($"获取{typeof(TModel).Name}实体类的指定的特性字段失败：{ex.Message}");
            }
        }

    }
}
