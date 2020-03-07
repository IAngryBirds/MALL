using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ILBLI.SqlSugar
{
    public interface IDBRepository<T> where T :class ,new()
    { 

        #region 删除操作

        /// <summary>
        /// 根据实体主键删除
        /// </summary>
        /// <typeparam name="PkType"></typeparam>
        /// <param name="pk">单个主键</param>
        /// <returns></returns>
        public bool DeleteKey<PkType>(PkType pk);

        /// <summary>
        /// 根据实体类（主键必须有值）进行删除数据
        /// </summary>
        /// <param name="deleteObjs">要删除的实体</param>
        /// <returns></returns>
        public bool Delete(T deleteObj);

        /// <summary>
        /// 根据实体主键集合删除
        /// </summary>
        /// <typeparam name="PkType"></typeparam>
        /// <param name="pks">主键值集合</param>
        /// <returns></returns>
        public bool DeleteKey<PkType>(List<PkType> pks);

        /// <summary>
        /// 根据实体类（主键必须有值）批量删除
        /// </summary>
        /// <param name="deleteObjs">要删除的实体集</param>
        /// <returns></returns>
        public bool Delete(List<T> deleteObjs);

        /// <summary>
        /// 根据Lambda表达式进行删除
        /// </summary>
        /// <param name="expression">删除数据表达式</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 根据自定义的字段删除数据
        /// </summary>
        /// <typeparam name="PkType"></typeparam>
        /// <param name="inField">按自定义删除的字段表达式</param>
        /// <param name="primaryKeyValues">批量删除的该字段集合</param>
        /// <returns></returns>
        public bool Delete<PkType>(Expression<Func<T, object>> inField, List<PkType> primaryKeyValues);

        #endregion

        #region 插入操作

        /// <summary>
        /// 批量插入实体类集合
        /// </summary>
        /// <param name="insertObjs">实体类集合</param>
        /// <returns></returns>
        public int Inset(T insertObj);

        /// <summary>
        /// 批量插入实体类集合
        /// </summary>
        /// <param name="insertObjs">实体类集合</param>
        /// <returns></returns>
        public int Inset(List<T> insertObjs);

        /// <summary>
        /// 批量插入实体类集合（是否忽略NULL值）【需要验证是否支持批量插入】
        /// </summary>
        /// <param name="insertObjs">需要插入的值</param>
        /// <param name="ignoreNullColums">是否忽略null值 eg: true 表示忽略，false 表示不忽略</param>
        /// <returns></returns>
        public int Inset(List<T> insertObjs, bool ignoreNullColums);

        /// <summary>
        /// 批量插入实体类集合（自定义需要忽略的字段）【需要验证是否支持批量插入】
        /// </summary>
        /// <param name="insertObjs">需要插入的值</param>
        /// <param name="ignoreNullColums">需要忽略的字段eg:it => new { it.Name, it.TestId }</param>
        /// <returns></returns>
        public int Inset(List<T> insertObjs, Expression<Func<T, object>> columns);

        #endregion

        #region 更新操作

        /// <summary>
        /// 根据实体类的主键（主键必须有值）批量更新数据
        /// </summary>
        /// <param name="updateObj">需更新数据</param>
        /// <returns></returns>
        public int Update(T updateObj);

        /// <summary>
        /// 根据实体类的主键（主键必须有值）批量更新数据
        /// </summary>
        /// <param name="updateObjs">需更新数据集合</param>
        /// <returns></returns>
        public int Update(List<T> updateObjs);

        /// <summary>
        /// 根据实体类的主键（主键必须有值）批量更新指定列的数据【需要验证是否支持批量更新】
        /// </summary>
        /// <param name="updateObjs">需要更新的数据集</param>
        /// <param name="columns">需要更新的列</param>
        /// <returns></returns>
        public int UpdateColumns(List<T> updateObjs, Expression<Func<T, object>> columns);

        /// <summary>
        /// 根据实体类的主键（主键必须有值）批量更新除指定列外的数据【需要验证是否支持批量更新】
        /// </summary>
        /// <param name="updateObjs">需要更新的数据集</param>
        /// <param name="columns">需要忽略更新的列</param>
        /// <returns></returns>
        public int UpdateIgnoreColumns(List<T> updateObjs, Expression<Func<T, object>> columns);

        /// <summary>
        /// 根据指定的字段批量更新数据【需要验证是否支持批量更新】
        /// </summary>
        /// <param name="updateObjs">需更新的数据集</param>
        /// <param name="columns">依据这些字段作为更新条件</param>
        /// <returns></returns>
        public int UpdateWhere(List<T> updateObjs, Expression<Func<T, object>> columns);

        /// <summary>
        /// 根据指定的字段批量更新指定字段的数据【需要验证是否支持批量更新】
        /// </summary>
        /// <param name="updateObjs">需更新的数据集</param>
        /// <param name="updateColumns">更新这些指定的字段（注意：这个必须包含whereColumns的列）</param>
        /// <param name="whereColumns">依据这些字段作为更新条件</param>
        /// <returns></returns>
        public int UpdateWhereColumns(List<T> updateObjs, Expression<Func<T, object>> updateColumns, Expression<Func<T, object>> whereColumns);


        #endregion

        #region 查询操作


        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public List<T> FindAll();

        /// <summary>
        /// 查询前N条数据
        /// </summary>
        /// <returns></returns>
        public List<T> FindTop(int topNum);

        /// <summary>
        /// 根据主键查询数据
        /// </summary>
        /// <param name="pk">主键值</param>
        /// <returns></returns>
        public T FindKey(object pk);

        /// <summary>
        /// 根据自定义的条件查询数据
        /// </summary>
        /// <param name="whereString">where语句</param>
        /// <param name="parameters">查询的参数</param>
        /// <returns></returns>
        public List<T> FindWhere(string whereString, object parameters = null);

        /// <summary>
        /// 根据自定义的条件查询数据返回匿名类字段
        /// </summary>
        /// <param name="whereString">where语句</param>
        /// <param name="parameters">查询的参数</param>
        /// <param name="expression">需要返回的匿名类</param>
        /// <returns></returns>
        public List<dynamic> FindWhere(string whereString, Expression<Func<T, dynamic>> expression, object parameters = null);

        /// <summary>
        /// 根据指定的字段查询数据
        /// </summary>
        /// <typeparam name="FieldType"></typeparam>
        /// <param name="expression">自定义字段</param>
        /// <param name="inValues">字段取值集合</param>
        /// <returns></returns>
        public List<T> FindIn<FieldType>(Expression<Func<T, object>> expression, List<FieldType> inValues);

        /// <summary>
        /// 是否存在数据【需要校验是否支持参数化查询】
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public bool IsExits(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereString">自定义的where查询语句</param> 
        /// <param name="pageIndex">第n页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="parameters">参数化查询参数</param>
        /// <returns></returns>
        public Tuple<List<T>, int> QueryPage(string whereString, int pageIndex, int pageSize, object parameters = null);

        /// <summary>
        /// 分页查询(自定义返回的匿名类字段)
        /// </summary>
        /// <param name="whereString">自定义的where查询语句</param>
        /// <param name="expression">需要返回的匿名类</param>
        /// <param name="pageIndex">第n页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="parameters">参数化查询参数</param>
        /// <returns></returns>
        public Tuple<List<dynamic>, int> QueryPage(string whereString, Expression<Func<T, dynamic>> expression, int pageIndex, int pageSize, object parameters = null);


        #endregion

        #region 事务操作



        #endregion

    }
}
