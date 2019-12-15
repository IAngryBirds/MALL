using System;
using System.ComponentModel.DataAnnotations;

namespace ILBLI.Model
{
    /// <summary>
    /// 商品表
    /// </summary>
    public class GoodsEntity  
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
        /// 名称
        /// <summary>
        public string Name { set; get; }
        /// <summary>
        /// 注释
        /// <summary>
        public string Memo { set; get; }
        /// <summary>
        /// 类目ID
        /// <summary>
        public int CategoryID { set; get; }
        /// <summary>
        /// 类目名称
        /// <summary>
        public string CategoryName { set; get; }
        /// <summary>
        /// 价格
        /// <summary>
        public double Price { set; get; }
        /// <summary>
        /// 库存
        /// <summary>
        public double Reserve { set; get; }
        /// <summary>
        /// 是否启用 0禁用1启用
        /// <summary>
        public int IsEnable { set; get; }
        /// <summary>
        /// 创建日期 默认为当前时间
        /// <summary>
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 更新时间
        /// <summary>
        public DateTime UpdateTime { set; get; }
        /// <summary>
        /// 排序号
        /// <summary>
        public double OrderNo { set; get; }
        /// <summary>
        /// 单位
        /// <summary>
        public string Unit { set; get; }
    }
}
