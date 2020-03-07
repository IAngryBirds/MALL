using System;
using System.ComponentModel.DataAnnotations;

namespace ILBLI.Unity
{
    /// <summary>
    /// 分页请求Request
    /// </summary>
    public class Page
    {
        /// <summary>
        /// 每页数据条数[分页查询必填项]
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "每页显示条数须大于0")]
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页码（当前第几页）[分页查询必填项]
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "当前页码须大于0")]
        public int PageIndex { get; set; }
    }
}
