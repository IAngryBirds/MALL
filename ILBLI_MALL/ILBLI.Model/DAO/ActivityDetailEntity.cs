using System;
using System.ComponentModel.DataAnnotations;

namespace ILBLI.Model
{
    /// <summary>
    /// 活动明细表
    /// </summary>
    public class ActivityDetailEntity
    {
        /// <summary>
        /// 编号
        /// <summary>
        [Key]
        public int ID { set; get; }
        /// <summary>
        /// 活动编号
        /// <summary>
        public int ActivityID { set; get; }
        /// <summary>
        /// 商品编号
        /// <summary>
        public int GoodsID { set; get; }
        /// <summary>
        /// 创建日期 默认为当前时间
        /// <summary>
        public DateTime CreateDate { set; get; }
        /// <summary>
        /// 排序号
        /// <summary>
        public double OrderNo { set; get; }



    }
}
