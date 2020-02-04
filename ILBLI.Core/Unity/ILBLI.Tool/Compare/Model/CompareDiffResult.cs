using System.Collections.Generic;

namespace ILBLI.Tool
{
    /// <summary>
    /// 比较两个实体类的差异部分数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CompareDiffResult<T> where T : class
    {
        /// <summary>
        /// 比较差异部分数据
        /// </summary>
        public CompareDiffResult()
        {
            InsertList = new List<T>();
            UpdateList = new List<T>();
            DeleteList = new List<T>();
            InsertList = new List<T>();
        }
        /// <summary>
        /// 需插入的数据
        /// </summary>
        public List<T> InsertList { get; set; }
        /// <summary>
        /// 需更新的数据
        /// </summary>
        public List<T> UpdateList { get; set; }
        /// <summary>
        /// 需删除的数据
        /// </summary>
        public List<T> DeleteList { get; set; }
        /// <summary>
        /// 需忽视的数据
        /// </summary>
        public List<T> IngoreList { get; set; }
    }
}
