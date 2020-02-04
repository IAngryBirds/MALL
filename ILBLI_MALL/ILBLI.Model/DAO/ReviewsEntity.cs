using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILBLI.Model
{
    /// <summary>
    /// 商品评价
    /// </summary>
    public class ReviewsEntity
    {
        /// <summary>
        /// 编号
        /// <summary>
        [Key]
        public int ID { set; get; }
        /// <summary>
        /// 商品编号
        /// <summary>
        public int GoodsID { set; get; }
        /// <summary>
        /// 消费者ID 评价人
        /// <summary>
        public int ConsumerID { set; get; }
        /// <summary>
        /// 是否匿名 0否1是
        /// <summary>
        public int IsAnonymous { set; get; }
        /// <summary>
        /// 注释 用户评价
        /// <summary>
        public string Memo { set; get; }
        /// <summary>
        /// 创建日期 默认为当前时间
        /// <summary> 
        public DateTime CreateTime { set; get; }



    }
}
