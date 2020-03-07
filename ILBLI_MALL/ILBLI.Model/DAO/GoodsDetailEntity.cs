using System.ComponentModel.DataAnnotations;

namespace ILBLI.Model
{

    /// <summary>
    /// 商品明细详情表
    /// </summary>
    public class GoodsDetailEntity
    {
        /// <summary>
        /// 商品编号
        /// <summary>
        [Key]
        public int ID { set; get; }
        /// <summary>
        /// 注释 商品详情介绍
        /// <summary>
        public string Memo { set; get; }



    }

}
