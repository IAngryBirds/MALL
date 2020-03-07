﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ILBLI.Model
{
    /// <summary>
    /// 商家表
    /// </summary>
    public class ShopEntity
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
        public string Memo { set; get; }
        /// <summary>
        /// 创建日期 默认为当前时间
        /// <summary>
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 排序号
        /// <summary>
        public double OrderNo { set; get; }



    }
}
