using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ILBLI.SqlSugar
{
    public class DBRepository<T> : IDBRepository<T> where T : class, new()
    {
        /// <summary>
        /// 读取配置文件的Config信息
        /// </summary>
        private readonly ConnectionConfig _Config;
        private readonly ILogger _Log = null;
        protected DBRepository(IOptions<ConnectionConfig> config, ILogger<DBRepository<T>> logger)
        {
            if(config==null)
            {
                throw new ArgumentException("数据库配置尚未初始化...");
            }
            _Config = config.Value;
            /*
             * 这里使用主从读写分离有问题，HitRate配置无法生效，
             * 如果需要使用HitRate进行权重分配，可以在这里进行赋值，对各个从库进行权重配置
             *  if(_Config.SlaveConnectionConfigs!=null && _Config.SlaveConnectionConfigs.Count > 0)
                {
                    _Config.SlaveConnectionConfigs.ForEach(x => { x.HitRate = 10; }); 
                }
             */

            _Log = logger;
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        protected SqlSugarClient GetClient()
        {
            return new SqlSugarClient(_Config);
        }

        #region 删除操作

        /// <summary>
        /// 根据实体主键删除
        /// </summary>
        /// <typeparam name="PkType"></typeparam>
        /// <param name="pk">单个主键</param>
        /// <returns></returns>
        public bool DeleteKey<PkType>(PkType pk)
        {
            List<PkType> pks = new List<PkType> { pk };
            return this.DeleteKey(pks);
        }

        /// <summary>
        /// 根据实体类（主键必须有值）进行删除数据
        /// </summary>
        /// <param name="deleteObjs">要删除的实体</param>
        /// <returns></returns>
        public bool Delete(T deleteObj)
        {
            List<T> deleteObjs = new List<T> { deleteObj };
            return this.Delete(deleteObjs);
        }

        /// <summary>
        /// 根据实体主键集合删除
        /// </summary>
        /// <typeparam name="PkType"></typeparam>
        /// <param name="pks">主键值集合</param>
        /// <returns></returns>
        public bool DeleteKey<PkType>(List<PkType> pks) 
        {
            using (SqlSugarClient db = this.GetClient())
            {
                var result = db.Deleteable<T>().In(pks).ExecuteCommand();
            }
            return true;
        }
         
        /// <summary>
        /// 根据实体类（主键必须有值）批量删除
        /// </summary>
        /// <param name="deleteObjs">要删除的实体集</param>
        /// <returns></returns>
        public bool Delete(List<T> deleteObjs)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                var result = db.Deleteable<T>().Where(deleteObjs).ExecuteCommand();
            }
            return true;
        }

        /// <summary>
        /// 根据Lambda表达式进行删除
        /// </summary>
        /// <param name="expression">删除数据表达式</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> expression)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                var result = db.Deleteable<T>().Where(expression).ExecuteCommand();
            }
            return true;
        }

        /// <summary>
        /// 根据自定义的字段删除数据
        /// </summary>
        /// <typeparam name="PkType"></typeparam>
        /// <param name="inField">按自定义删除的字段表达式</param>
        /// <param name="primaryKeyValues">批量删除的该字段集合</param>
        /// <returns></returns>
        public bool Delete<PkType>(Expression<Func<T, object>> inField, List<PkType> primaryKeyValues)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                var result = db.Deleteable<T>().In(inField, primaryKeyValues).ExecuteCommand();
            }
            return true;
        }
         
        #endregion
        
        #region 插入操作

        /// <summary>
        /// 批量插入实体类集合
        /// </summary>
        /// <param name="insertObjs">实体类集合</param>
        /// <returns></returns>
        public int Inset(T insertObj)
        {
            List<T> insertObjs = new List<T> { insertObj };
            return this.Inset(insertObjs);
        }

        /// <summary>
        /// 批量插入实体类集合
        /// </summary>
        /// <param name="insertObjs">实体类集合</param>
        /// <returns></returns>
        public int Inset(List<T> insertObjs)
        {
            using (SqlSugarClient db = this.GetClient())
            {
               /* db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    _Log.LogError(db.Ado.Connection.ConnectionString);
                };*/
                return db.Insertable(insertObjs).ExecuteCommand();
            }
        }

        /// <summary>
        /// 批量插入实体类集合（是否忽略NULL值）【需要验证是否支持批量插入】
        /// </summary>
        /// <param name="insertObjs">需要插入的值</param>
        /// <param name="ignoreNullColums">是否忽略null值 eg: true 表示忽略，false 表示不忽略</param>
        /// <returns></returns>
        public int Inset(List<T> insertObjs,bool ignoreNullColums)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                return db.Insertable(insertObjs).IgnoreColumns(ignoreNullColums).ExecuteCommand();
            }
        }

        /// <summary>
        /// 批量插入实体类集合（自定义需要忽略的字段）【需要验证是否支持批量插入】
        /// </summary>
        /// <param name="insertObjs">需要插入的值</param>
        /// <param name="ignoreNullColums">需要忽略的字段eg:it => new { it.Name, it.TestId }</param>
        /// <returns></returns>
        public int Inset(List<T> insertObjs, Expression<Func<T, object>> columns)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                return db.Insertable(insertObjs).IgnoreColumns(columns).ExecuteCommand();
            }
        }


        #endregion

        #region 更新操作

        /// <summary>
        /// 根据实体类的主键（主键必须有值）批量更新数据
        /// </summary>
        /// <param name="updateObj">需更新数据</param>
        /// <returns></returns>
        public int Update(T updateObj)
        {
            List<T> updateObjs = new List<T> { updateObj };
            return this.Update(updateObjs);
        }

        /// <summary>
        /// 根据实体类的主键（主键必须有值）批量更新数据
        /// </summary>
        /// <param name="updateObjs">需更新数据集合</param>
        /// <returns></returns>
        public int Update(List<T> updateObjs)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                return db.Updateable(updateObjs).ExecuteCommand();
            }
        }

        /// <summary>
        /// 根据实体类的主键（主键必须有值）批量更新指定列的数据【需要验证是否支持批量更新】
        /// </summary>
        /// <param name="updateObjs">需要更新的数据集</param>
        /// <param name="columns">需要更新的列</param>
        /// <returns></returns>
        public int UpdateColumns(List<T> updateObjs, Expression<Func<T, object>> columns)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                return db.Updateable(updateObjs).UpdateColumns(columns).ExecuteCommand();
            }
        }

        /// <summary>
        /// 根据实体类的主键（主键必须有值）批量更新除指定列外的数据【需要验证是否支持批量更新】
        /// </summary>
        /// <param name="updateObjs">需要更新的数据集</param>
        /// <param name="columns">需要忽略更新的列</param>
        /// <returns></returns>
        public int UpdateIgnoreColumns(List<T> updateObjs, Expression<Func<T, object>> columns)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                return db.Updateable(updateObjs).IgnoreColumns(columns).ExecuteCommand();
            }
        }
        
        /// <summary>
        /// 根据指定的字段批量更新数据【需要验证是否支持批量更新】
        /// </summary>
        /// <param name="updateObjs">需更新的数据集</param>
        /// <param name="columns">依据这些字段作为更新条件</param>
        /// <returns></returns>
        public int UpdateWhere(List<T> updateObjs, Expression<Func<T, object>> columns)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                return db.Updateable(updateObjs).WhereColumns(columns).ExecuteCommand();
            }
        }

        /// <summary>
        /// 根据指定的字段批量更新指定字段的数据【需要验证是否支持批量更新】
        /// </summary>
        /// <param name="updateObjs">需更新的数据集</param>
        /// <param name="updateColumns">更新这些指定的字段（注意：这个必须包含whereColumns的列）</param>
        /// <param name="whereColumns">依据这些字段作为更新条件</param>
        /// <returns></returns>
        public int UpdateWhereColumns(List<T> updateObjs, Expression<Func<T, object>> updateColumns, Expression<Func<T, object>> whereColumns)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                return db.Updateable(updateObjs).UpdateColumns(updateColumns).WhereColumns(whereColumns).ExecuteCommand();
            }
        }


        #endregion

        #region 查询操作


        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public List<T> FindAll()
        {
            using (SqlSugarClient db = this.GetClient())
            {
               /* db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    _Log.LogError(db.Ado.Connection.ConnectionString);
                };*/
                return db.Queryable<T>().ToList();
            }
        }

        /// <summary>
        /// 查询前N条数据
        /// </summary>
        /// <returns></returns>
        public List<T> FindTop(int topNum)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                return db.Queryable<T>().Take(topNum).ToList();
            }
        }

        /// <summary>
        /// 根据主键查询数据
        /// </summary>
        /// <param name="pk">主键值</param>
        /// <returns></returns>
        public T FindKey(object pk)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                return db.Queryable<T>().InSingle(pk);
            }
        }

        /// <summary>
        /// 根据自定义的条件查询数据
        /// </summary>
        /// <param name="whereString">where语句</param>
        /// <param name="parameters">查询的参数</param>
        /// <returns></returns>
        public List<T> FindWhere(string whereString, object parameters = null)
        { 
            using (SqlSugarClient db = this.GetClient())
            {
                var sugarQueryable = db.Queryable<T>().Where(whereString, parameters);
                //加入自己自定义的特性扩展
                sugarQueryable = sugarQueryable.QueryableHandleAttribute();
                return sugarQueryable.ToList();
            }
        }

        /// <summary>
        /// 根据自定义的条件查询数据返回匿名类字段
        /// </summary>
        /// <param name="whereString">where语句</param>
        /// <param name="parameters">查询的参数</param>
        /// <param name="expression">需要返回的匿名类</param>
        /// <returns></returns>
        public List<dynamic> FindWhere(string whereString, Expression<Func<T, dynamic>> expression,object parameters = null)
        { 
            using (SqlSugarClient db = this.GetClient())
            {
                var sugarQueryable = db.Queryable<T>().Where(whereString, parameters);
                //加入自己自定义的特性扩展
                sugarQueryable = sugarQueryable.QueryableHandleAttribute();
                return sugarQueryable.Select(expression).ToList(); 
            }
        }

        /// <summary>
        /// 根据指定的字段查询数据
        /// </summary>
        /// <typeparam name="FieldType"></typeparam>
        /// <param name="expression">自定义字段</param>
        /// <param name="inValues">字段取值集合</param>
        /// <returns></returns>
        public List<T> FindIn<FieldType>(Expression<Func<T, object>> expression, List<FieldType> inValues)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                return db.Queryable<T>().In(expression, inValues).ToList() ;
            }
        }

        /// <summary>
        /// 是否存在数据【需要校验是否支持参数化查询】
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public bool IsExits(Expression<Func<T, bool>> expression)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                return db.Queryable<T>().Any(expression);
            }
        }
         
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereString">自定义的where查询语句</param> 
        /// <param name="pageIndex">第n页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="parameters">参数化查询参数</param>
        /// <returns></returns>
        public Tuple<List<T>, int> QueryPage(string whereString,int pageIndex, int pageSize, object parameters = null)
        {
            int pageCounts = 0;
            using (SqlSugarClient db = this.GetClient())
            {
                var sugarQueryable = db.Queryable<T>().Where(whereString, parameters);
                //加入自己自定义的特性扩展
                sugarQueryable = sugarQueryable.QueryableHandleAttribute();
                List<T> querys = sugarQueryable.ToPageList(pageIndex, pageSize, ref pageCounts);
                return Tuple.Create(querys, pageCounts);
            }
        }

        /// <summary>
        /// 分页查询(自定义返回的匿名类字段)
        /// </summary>
        /// <param name="whereString">自定义的where查询语句</param>
        /// <param name="expression">需要返回的匿名类</param>
        /// <param name="pageIndex">第n页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="parameters">参数化查询参数</param>
        /// <returns></returns>
        public Tuple<List<dynamic>,int> QueryPage(string whereString, Expression<Func<T, dynamic>> expression,int pageIndex,int pageSize,object parameters = null)
        {
            int pageCounts = 0;
            using(SqlSugarClient db = this.GetClient())
            {
                var sugarQueryable = db.Queryable<T>().Where(whereString, parameters);
                //加入自己自定义的特性扩展
                sugarQueryable = sugarQueryable.QueryableHandleAttribute();
                List<dynamic> querys = sugarQueryable.Select(expression).ToPageList(pageIndex, pageSize, ref pageCounts);
                return Tuple.Create(querys, pageCounts);
            } 
        }


        #endregion

        #region 事务操作

        /// <summary>
        /// 使用原生的事务[待验证]
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool TranAdo(Action<IAdo> action)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                try
                {
                    db.Ado.BeginTran();
                    
                    action(db.Ado);
                    
                    db.Ado.CommitTran();
                }catch(Exception ex)
                {
                    db.Ado.RollbackTran();
                    throw;
                }
            }
            return true;
        }

        /// <summary>
        /// 使用封装的事务[待验证]
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool UseTran(Action<SqlSugarClient> action)
        {
            using (SqlSugarClient db = this.GetClient())
            {
                db.Ado.UseTran(() =>
                {
                    action(db);
                });
            }
            return true;
        }

        #endregion

    }
}
