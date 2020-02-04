using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ILBLI.Core
{
    /// <summary>
    /// 实体类转换
    /// </summary>
    public static class EntityHelper
    {

        #region 实体类集合转换成DataTable

        /// <summary>
        /// 实体类集合转DataSet
        /// </summary>
        /// <param name="tList">待转换实体类集合</param>
        /// <returns>转换后DataSet</returns>
        public static DataSet EntityToDataSet<T>(List<T> tList)
        {
            if (tList == null || tList.Count == 0)
            {
                return null;
            }
            else
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(EntityToDataTable(tList));
                return ds;
            }
        }

        /// <summary>
        /// 实体类集合转DataTable
        /// </summary>
        /// <param name="tList">待转换实体类集合</param>
        /// <returns>转换后DataTable</returns>
        public static DataTable EntityToDataTable<T>(List<T> tList)
        {
            if (tList == null || tList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateDataTable(tList[0]);
            foreach (var t in tList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (var propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(t, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 实体类转表结构
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns>转换后DataTable</returns>
        private static DataTable CreateDataTable<T>(T t)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }

        /// <summary>
        /// 字典转表结构
        /// </summary>
        /// <param name="dicInfo"></param>
        /// <returns></returns>
        public static DataTable DictionaryToDataTable(List<Dictionary<string, string>> dicInfo)
        {
            DataTable dt = new DataTable();
            try
            {
                DataColumn dc;
                DataRow newRow;
                dc = new DataColumn(); //dt.Columns.Add("ID", Type.GetType("System.Int32"));
                dc.AutoIncrement = true;//自动增加
                dc.AutoIncrementSeed = 1;//起始为1
                dc.AutoIncrementStep = 1;//步长为1
                dc.AllowDBNull = false;//不允许为空
                foreach (var item in dicInfo)//将所有数据的键全部保存到DataTable里去
                {
                    foreach (var item_1 in item)
                    {
                        if (!dt.Columns.Contains(item_1.Key))
                        {
                            dt.Columns.Add(item_1.Key, Type.GetType("System.String"));
                        }
                    }
                }
                foreach (var item in dicInfo)
                {
                    newRow = dt.NewRow();
                    foreach (var item_1 in item)
                    {
                        newRow[item_1.Key] = item_1.Value;
                    }
                    dt.Rows.Add(newRow);
                }
            }
            catch { }
            return dt;
        }

        #endregion 实体类集合转换成DataTable

        #region 实体类转实体类

        /// <summary>
        /// Model实体类比较赋值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="OutEntity">要修改赋值修改的实体</param>
        /// <param name="IntoEntity">传入的实体</param>
        /// <param name="NotField">不用比较赋值的字段，一般是主键</param>
        /// <returns></returns>
        public static T EntityToEntity<T>(this T OutEntity, T IntoEntity, string[] NotField)
        {
            try
            {
                Type infotype = typeof(T);
                PropertyInfo[] propertys = infotype.GetProperties();
                foreach (PropertyInfo p in propertys)
                {
                    if (p.DeclaringType != infotype) continue;
                    string fieldname = p.Name;
                    if (NotField != null && NotField.Contains(fieldname)) continue;
                    object value = infotype.GetProperty(fieldname).GetValue(IntoEntity, null);
                    if (value == null) continue;
                    infotype.GetProperty(fieldname).SetValue(OutEntity, value, null);
                }
                return OutEntity;
            }
            catch
            {
                return default(T);
            }
        }

         
        /// <summary>
        /// 实体类的转换
        /// </summary>
        /// <typeparam name="T">待转换的类</typeparam>
        /// <typeparam name="TModel">转换后的类</typeparam>
        /// <param name="model">待转换的实体类数据</param>
        /// <param name="dicTurn">映射字典 key:待转换字段 ，value：转换后字段</param>
        /// <returns></returns>
        public static TModel EntityToEntity<T, TModel>(this T model, Dictionary<string, string> dicTurn)
        {
            Type t = typeof(TModel);
            TModel resModel = (TModel)Activator.CreateInstance(t); //动态创建需要返回的实体类

            if (model == null)
                return resModel;

            PropertyInfo[] properties = model.GetType().GetProperties();//获取待转换的实体类属性

            if (properties == null)
                return resModel;

            string outName;
            object outValue;
            foreach (var item in properties)//遍历属性值
            {
                dicTurn.TryGetValue(item.Name, out outName);//尝试从映射字典中获取值

                if (string.IsNullOrWhiteSpace(outName))
                    continue;

                outValue = item.GetValue(model);//尝试从待转换实体类中取值
                if (outValue == null)
                    continue;

                t.GetProperty(outName)?.SetValue(resModel, outValue);//给对象实体类赋值 

            }

            return resModel;
        }

        #endregion




    }
}
