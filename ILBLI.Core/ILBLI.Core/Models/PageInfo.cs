namespace ILBLI.Unity
{
    /// <summary>
    /// 分页返回Response
    /// </summary>
    public class PageInfo :Page
    {
        /// <summary>
        /// 默认的构造函数
        /// </summary>
        /// <param name="page">前端分页查询</param>
        /// <param name="recordCount">数据总条数</param>
        public PageInfo(Page page, int recordCount)
        {
            PageSize = page.PageSize;
            PageIndex = page.PageIndex;
            RecordCount = recordCount;
            PageCount = RecordCount / PageSize + (RecordCount % PageSize > 0 ? 1 : 0);
        }
         
        /// <summary>
        /// 记录总条数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
        
    }
}
