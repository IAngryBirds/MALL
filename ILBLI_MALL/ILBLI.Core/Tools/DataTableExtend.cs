using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ILBLI.Core
{
    /// <summary>
    /// DataTable扩展类，用于将datatable转换为实体类
    /// </summary>
    public static class DataTableExtend 
    {
        /// <summary>
        /// 将DataTable转换为实体类集合【简易版】
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<TModel> ConvertToModel<TModel>(this DataTable dt)  where TModel :new()
        {
            // 定义集合    
            IList<TModel> ts = new List<TModel>();
            foreach (DataRow dr in dt.Rows)
            {
                TModel t = new TModel();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    string tempName = "";
                    tempName = pi.Name;  // 检查DataTable是否包含此列    
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (!pi.PropertyType.IsGenericType)
                        {
                            //非泛型
                            pi.SetValue(t, value == DBNull.Value ? null : Convert.ChangeType(value, pi.PropertyType), null);
                        }
                        else
                        {
                            //泛型Nullable<>
                            Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                pi.SetValue(t, value == DBNull.Value ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(pi.PropertyType)), null);
                            }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 将dataTable转换为实体类集合【详细版】
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <param name="excludePropertys">需要排除转换的属性</param>
        /// <returns></returns>
        public static IList<TModel> DataTableToList<TModel>(this DataTable dt , string[] excludePropertys) where TModel :new ()
        {
            string propertyName = string.Empty;
            string propertyValue = string.Empty;
            try
            {
                IList<TModel> list = new List<TModel>();
                // 获取所有属性
                PropertyInfo[] propertys = new TModel().GetType().GetProperties();
                TModel model = default(TModel);
                // 循环所有数据
                foreach (DataRow row in dt.Rows)
                {
                    model = new TModel();
                    for (int i = 0; i < propertys.Length; i++)
                    {
                        propertyName = propertys[i].Name;

                        // 如果存在排出属性，那么该属性不需要参数化
                        if (excludePropertys != null && excludePropertys.Contains(propertyName))
                        {
                            continue;
                        }
                        if (propertys[i].CanWrite && dt.Columns.Contains(propertys[i].Name) && row[propertys[i].Name] != DBNull.Value)
                        {
                            propertyValue = Convert.ToString(row[propertys[i].Name]);

                            if (propertys[i].PropertyType.Name.Equals("Nullable`1"))
                            {
                                if (propertys[i].PropertyType.FullName.Contains("System.Int32"))
                                {
                                    if (!string.IsNullOrEmpty(propertyValue))
                                    {
                                        propertys[i].SetValue(model, Convert.ToInt32(propertyValue));
                                    }
                                    else
                                    {
                                        propertys[i].SetValue(model, 0);
                                    }
                                }
                                else if (propertys[i].PropertyType.FullName.Contains("System.DateTime"))
                                {
                                    if (string.IsNullOrEmpty(propertyValue) || Convert.ToDateTime(propertyValue) == DateTime.MinValue)
                                    {
                                        propertys[i].SetValue(model, DBNull.Value);
                                    }
                                    else
                                    {
                                        propertys[i].SetValue(model, Convert.ToDateTime(propertyValue));
                                    }
                                }
                                else if (propertys[i].PropertyType.FullName.Contains("System.Decimal"))
                                {
                                    propertys[i].SetValue(model, Convert.ToDecimal(propertyValue));
                                }
                                else if (propertys[i].PropertyType.FullName.Contains("System.Double"))
                                {
                                    propertys[i].SetValue(model, Convert.ToDouble(propertyValue));
                                }
                            }
                            else
                            {
                                switch (propertys[i].PropertyType.FullName)
                                {
                                    case "System.Guid":
                                        propertys[i].SetValue(model, new Guid(propertyValue));
                                        break;
                                    case "System.Int32":
                                        if (!string.IsNullOrEmpty(propertyValue))
                                        {
                                            propertys[i].SetValue(model, Convert.ToInt32(propertyValue));
                                        }
                                        else
                                        {
                                            propertys[i].SetValue(model, 0);
                                        }

                                        break;
                                    case "System.DateTime":
                                        if (string.IsNullOrEmpty(propertyValue) || Convert.ToDateTime(propertyValue) == DateTime.MinValue)
                                        {
                                            propertys[i].SetValue(model, DBNull.Value);
                                        }
                                        else
                                        {
                                            propertys[i].SetValue(model, Convert.ToDateTime(propertyValue));
                                        }
                                        break;
                                    case "System.Decimal":
                                        propertys[i].SetValue(model, Convert.ToDecimal(propertyValue));
                                        break;
                                    case "System.Double":
                                        propertys[i].SetValue(model, Convert.ToDouble(propertyValue));
                                        break;
                                    default:
                                        propertys[i].SetValue(model, propertyValue);
                                        break;
                                }
                            }

                        }
                    }
                    list.Add(model);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("DataTable转列表时报错:参数名:{0}参数值:{1}\n报错信息:{2}", propertyName, propertyValue, ex.Message));
            }
        }

        /// <summary>
        /// 将DataTable转换为实体类集合【Emit版本】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToListByEmit<T>(this DataTable dt) where T : class, new()
        {
            List<T> list = new List<T>();
            if (dt == null || dt.Rows.Count == 0)
                return list;
            DataTableEntityBuilder<T> eblist = DataTableEntityBuilder<T>.CreateBuilder(dt.Rows[0]);
            foreach (DataRow info in dt.Rows)
                list.Add(eblist.Build(info));
            dt.Dispose();
            dt = null;
            return list;
        }
        public class DataTableEntityBuilder<Entity>
        {
            private static readonly MethodInfo getValueMethod = typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) });
            private static readonly MethodInfo isDBNullMethod = typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });
            private delegate Entity Load(DataRow dataRecord);
            private Load handler;
            private DataTableEntityBuilder() { }
            public Entity Build(DataRow dataRecord)
            {
                return handler(dataRecord);
            }
            public static DataTableEntityBuilder<Entity> CreateBuilder(DataRow dataRecord)
            {
                DataTableEntityBuilder<Entity> dynamicBuilder = new DataTableEntityBuilder<Entity>();
                DynamicMethod method = new DynamicMethod("DynamicCreateEntity", typeof(Entity), new Type[] { typeof(DataRow) }, typeof(Entity), true);
                ILGenerator generator = method.GetILGenerator();
                LocalBuilder result = generator.DeclareLocal(typeof(Entity));
                generator.Emit(OpCodes.Newobj, typeof(Entity).GetConstructor(Type.EmptyTypes));
                generator.Emit(OpCodes.Stloc, result);
                for (int i = 0; i < dataRecord.ItemArray.Length; i++)
                {
                    PropertyInfo propertyInfo = typeof(Entity).GetProperty(dataRecord.Table.Columns[i].ColumnName);
                    Label endIfLabel = generator.DefineLabel();
                    if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                    {
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldc_I4, i);
                        generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                        generator.Emit(OpCodes.Brtrue, endIfLabel);
                        generator.Emit(OpCodes.Ldloc, result);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldc_I4, i);
                        generator.Emit(OpCodes.Callvirt, getValueMethod);
                        generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                        generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                        generator.MarkLabel(endIfLabel);
                    }
                }
                generator.Emit(OpCodes.Ldloc, result);
                generator.Emit(OpCodes.Ret);
                dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
                return dynamicBuilder;
            }
        }

    }



}
