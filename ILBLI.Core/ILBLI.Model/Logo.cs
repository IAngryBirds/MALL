using ILBLI.SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace ILBLI.Model
{
    public class Logo
    {
        /// <summary>
        /// 品牌LOGOID
        /// </summary>
        [Key]
        public string LogoID { set; get; }
        /// <summary>
        /// 品牌LOGO中文名
        /// </summary>
        public string LogoName_CN { set; get; }
        /// <summary>
        /// 品牌LOGO英文名
        /// </summary>
        public string LogoName_EN { set; get; }
        /// <summary>
        /// 品牌LOGOURL
        /// </summary>
        public string LogoURL { set; get; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int SortNum { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [OrderByDesc]
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { set; get; }
    }



}
