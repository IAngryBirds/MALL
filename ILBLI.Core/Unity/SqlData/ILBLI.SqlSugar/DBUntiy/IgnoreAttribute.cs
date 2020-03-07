using System;

namespace ILBLI.SqlSugar
{
    /// <summary>
    /// 定义一个标记属性 ，可以再实体类属性上标记这个属性，然后再数据库映射SQL语句的时候可以自动过滤
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }
}
