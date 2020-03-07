namespace ILBLI.Unity
{
    public class PageResult :TResult
    {
        public PageInfo Page { get; set; }

        /// <summary>
        /// 返回查询带分页列表的成功结果
        /// </summary>
        /// <typeparam name="TModel">实体类型</typeparam>
        /// <param name="list">实体列表信息</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        public static PageResult PageSuccess<TModel>(TModel list, PageInfo pageInfo)
        {
            return new PageResult
            {  
                Data = list,
                Page = pageInfo
            };
        }
        
    }
}
