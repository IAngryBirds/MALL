using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILBLI.Model
{
    public class GoodsEntity  
    {

        /// <summary>
        /// 编号
        /// <summary>
        [Key]
        public int ID { set; get; }
        /// <summary>
        /// 名称
        /// <summary>
        public string Name { set; get; }
        /// <summary>
        /// 注释
        /// <summary>
        public string Desc { set; get; }
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
        public double Pice { set; get; }
        /// <summary>
        /// 库存
        /// <summary>
        public double Reserve { set; get; }
        /// <summary>
        /// 是否启用
        /// <summary>
        public bool IsEnable { set; get; }
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

    }
}
