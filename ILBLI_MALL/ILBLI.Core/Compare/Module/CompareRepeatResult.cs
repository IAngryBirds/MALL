using System.Collections.Generic;

namespace ILBLI.Core
{
    /// <summary>
    /// 比较实体类的重复数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CompareRepeatResult<T> where T : class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompareRepeatResult()
        {
            IsRepeat = false;
            RepeatList = new List<T>();
            RemoveRepeatList = new List<T>();
        }

        /// <summary>
        /// 是否重复 0 否 1 是
        /// </summary>
        public bool IsRepeat { get; set; }

        /// <summary>
        /// 重复的数据
        /// </summary>
        public List<T> RepeatList { get; set; }

        /// <summary>
        /// 去重复后的数据
        /// </summary>
        public List<T> RemoveRepeatList { get; set; }
    }
}
