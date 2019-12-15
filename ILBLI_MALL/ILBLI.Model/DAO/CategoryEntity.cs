using System;
using System.ComponentModel.DataAnnotations;

namespace ILBLI.Model
{
    /// <summary>
    /// 类目
    /// </summary>
    public class CategoryEntity
    {
        /// <summary>
        /// 编号
        /// <summary>
        [Key]
        public int ID { set; get; }
        /// <summary>
        /// 父编号
        /// <summary>
        public int Pid { set; get; }
        /// <summary>
        /// 类目路径
        /// <summary>
        public string Path { set; get; }
        /// <summary>
        /// 名称
        /// <summary>
        public string Name { set; get; }
        /// <summary>
        /// 注释
        /// <summary>
        public string Desc { set; get; }
        /// <summary>
        /// 创建日期
        /// <summary>
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 更新日期
        /// <summary>
        public DateTime UpdateTime { set; get; }
        /// <summary>
        /// 排序号
        /// <summary>
        public double OrderNo { set; get; }
        /// <summary>
        /// 是否启用
        /// <summary>
        public bool IsEnable { set; get; }



    }
}
