using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILBLI.Model
{
    public class GoodsModel:GoodsEntity
    {
        /// <summary>
        /// 商品缩略图（主图）
        /// </summary>
        public string Pic_url { get; set; }
        /// <summary>
        /// 评价数量
        /// </summary>
        public int ReviewsCounts { get; set; }
    }
}
