using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ILBLI.Core
{
    public class MySqlProvider : DataBaseCommon, ISQLProvider
    {
        public MySqlProvider() : base() { }
        public MySqlProvider(DBOperationMode mode) : base(mode) { }

        public IEnumerable<TModel> GetPageList<TModel>(string sqlStr, int pageIndex, int pageSize, out int counts)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据实体类获取分页数据
        /// </summary>
        /// <typeparam name="TModel">查询的实体类表</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="wherePairs">查询条件语句（目前只支持精确查询）</param>
        /// <param name="orderBy">排序语句</param>
        /// <param name="counts">返回记录总数</param>
        /// <returns></returns>
        public IEnumerable<TModel> GetPageList<TModel>(int pageIndex, int pageSize, out int counts, Dictionary<string, object> wherePairs = null, Dictionary<EOrderBy, string> orderByPairs = null)
        {
            counts = 0;

            List<PropertyInfo> list = DBPropertyHelper.GetPropertyInfo<TModel>().ToList();

            List<string> listQueryName = new List<string>();
            List<string> listParamName = new List<string>();
            string listOrderName = string.Empty;
            DynamicParameters paramValue = new DynamicParameters();

            list.ForEach(x => {
                listQueryName.Add(x.Name);
            });
            int startSize = (pageIndex - 1) * pageSize;

            WhereParametersHandler(wherePairs, listParamName, paramValue);//处理wherec查询参数

            listOrderName = OrderParametersHandler(orderByPairs);//处理排序查询参数

            string paramStr = listParamName.Count > 0 ? $" where { string.Join(" and ", listParamName) } " : string.Empty;//参数化查询字符串

            //string strSqlTemplate = "select {0} from {1} where 1=1 and {2} {3} LIMIT {4},{5} ; select found_rows() ";

            string strSql = $" select {string.Join(",", listQueryName)} " +
                                $"from {typeof(TModel).Name}{ paramStr} {listOrderName} " +
                                $"limit {startSize},{pageSize}  ";

            string strSqlCount = $" select count(1) from {typeof(TModel).Name }{ paramStr } ";

            using (IDbConnection conn = CreateDbOperation())
            {
                IEnumerable<TModel> result = conn.Query<TModel>(strSql, paramValue);
                counts = conn.Query<int>(strSqlCount, paramValue).FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// where参数化查询处理方法
        /// </summary>
        /// <param name="wherePairs"></param>
        /// <param name="listParamName"></param>
        /// <param name="paramValue"></param>
        private void WhereParametersHandler(Dictionary<string, object> wherePairs, List<string> listParamName, DynamicParameters paramValue)
        {
            if (wherePairs != null && wherePairs.Count > 0)
            {
                foreach (var item in wherePairs)
                {
                    listParamName.Add($"{item.Key}={_parameterChar}{item.Key}");//参数化查询拼接
                    paramValue.Add(item.Key, item.Value);//参数化查询赋值
                }
            }
        }

        /// <summary>
        /// order参数化查询处理方法
        /// </summary>
        /// <param name="orderPairs"></param>
        /// <param name="listParamName"></param>
        /// <param name="paramValue"></param>
        private string OrderParametersHandler(Dictionary<EOrderBy, string> orderByPairs)
        {
            string orderByStr = string.Empty;
            if (orderByPairs != null && orderByPairs.Count > 0)
            {
                foreach (var item in orderByPairs)
                {
                    if (item.Key == EOrderBy.ASC)
                        orderByStr += $" {item.Value} ASC ";
                    else
                        orderByStr += string.IsNullOrWhiteSpace(orderByStr) ? $"  {item.Value} DESC " : $" ,{item.Value} DESC ";
                }
            }
            return !string.IsNullOrWhiteSpace(orderByStr) ? $" order by {orderByStr} " : string.Empty;
        }


        #region 使用示例

        ///// <summary>
        ///// 获取分页信息
        ///// </summary>
        ///// <returns></returns>
        //public CommonResult GetList(DTO_Base_Page page)
        //{ 
        //    //Dictionary<string, object> dic = new Dictionary<string, object>();
        //    //dic.Add("SheetID", "A0201");
        //    //dic.Add("VisitorName", "张杰");
        //    //Dictionary<EOrderBy, string> order = new Dictionary<EOrderBy, string>();
        //    //order.Add(EOrderBy.ASC, "ID,SheetID");
        //    //order.Add(EOrderBy.DESC, "VisitorName");
        //    //dal.GetPageList<FK_Sheet>(page.PageIndex, page.PageSize, out int counts,dic,order);
        //    return new CommonResult();
        //}
        #endregion

    }
}
