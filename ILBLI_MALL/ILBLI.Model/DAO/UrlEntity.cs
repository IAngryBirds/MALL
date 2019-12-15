using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILBLI.Model
{
    public class UrlEntity
    {
        /// <summary>
        /// 商品链接
        /// </summary>
        public int GoodsID { get; set; }
        /// <summary>
        /// 图片显示地址
        /// </summary>
        public string Pic_url { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
