using System.Collections.Generic;

namespace ILBLI.Core
{
    public interface ISQLProvider
    {
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex">第N页</param>
        /// <param name="pageSize">每页显示条数</param>
        IEnumerable<TModel> GetPageList<TModel>(string sqlStr, int pageIndex, int pageSize, out int counts);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="pageIndex">第N页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="counts">返回记录总数</param>
        IEnumerable<TModel> GetPageList<TModel>(int pageIndex, int pageSize, out int counts, Dictionary<string, object> keyValuePairs, Dictionary<EOrderBy, string> orderBy);
         
    }
}
