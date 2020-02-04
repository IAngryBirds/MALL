using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILBLI.SqlSugar
{
    public static class SugarQueryableExtension
    {
        /// <summary>
        /// 对查询的命令进行自定义的属性扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sugarQueryable"></param>
        /// <returns></returns>
        public static ISugarQueryable<T> QueryableHandleAttribute<T>(this ISugarQueryable<T> sugarQueryable) where T : class, new()
        {
            /*
             * 这组反射一般可以拿到外边，通过传入的方式，这样可以提高数据库执行效率
             * 但是放在这里，可以做到更好的自定义与扩展
             */
            var propIgnores = PropertyExtension.GetPropertyByAttribute<T, IgnoreAttribute>()?.Select(x => x.Name);
            var orderByAscs = PropertyExtension.GetPropertyByAttribute<T, OrderByAttribute>()?.Select(x => x.Name);
            var orderByDescs = PropertyExtension.GetPropertyByAttribute<T, OrderByDescAttribute>()?.Select(x => x.Name);

            //对标记忽略的字段进行过滤
            if (propIgnores != null && propIgnores.Any())
            {
                sugarQueryable = sugarQueryable.IgnoreColumns(propIgnores.ToArray());
            }
            //按正序标记进行排序
            if (orderByAscs != null && orderByAscs.Any())
            {
                sugarQueryable = sugarQueryable.OrderBy($" {string.Join(",", orderByAscs)} asc ");
            }
            //按倒序标记进行排序
            if (orderByDescs != null && orderByDescs.Any())
            {
                sugarQueryable = sugarQueryable.OrderBy($" {string.Join(",", orderByDescs)} desc ");
            }

            return sugarQueryable;
        }
    }
}
