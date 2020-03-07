using System;
using System.ComponentModel.DataAnnotations;

namespace ILBLI.Model
{
    /// <summary>
    /// 轮播图
    /// </summary>
    public class BannerEntity 
    {
        /// <summary>
        /// 编号
        /// <summary>
        [Key]
        public int ID { set; get; }
        /// <summary>
        /// 商家码
        /// <summary>
        public int ShopID { set; get; }
        /// <summary>
        /// 注释
        /// <summary>
        public string Memo { set; get; }
        /// <summary>
        /// 排序号
        /// <summary>
        public double OrderNo { set; get; }
        /// <summary>
        /// 商品ID
        /// <summary>
        public int GoodsID { set; get; }
        /// <summary>
        /// 图片显示地址
        /// <summary>
        public string Pic_url { set; get; }
        /// <summary>
        /// 创建日期 默认为当前时间
        /// <summary>
        public DateTime CreateTime { set; get; }

    }
}
