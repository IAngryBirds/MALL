using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILBLI.Model
{
    public class DetailPage
    {
        /// <summary>
        /// 请求时间，查询的商品必须在这个日期之前，下拉刷新这个时间
        /// </summary>
        public DateTime RequestTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 当前页，上滑+1
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 查询类目（0-默认 1-价格 2-热销）
        /// </summary>
        public string Type { get; set; }

    }
}
