namespace ILBLI.Core.Log4
{
    /// <summary>
    /// 第一步：定义数据库操作日志实体类
    /// </summary>
    public class LogModel
    { 
        /// <summary>
      /// 事件类型
      /// </summary>
        public string EventLog { get; set; }
        /// <summary>
        /// 请求IP地址
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 操作系统
        /// </summary>
        public string OperationSystem { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperationType { get; set; }
        /// <summary>
        /// 操作说明
        /// </summary>
        public string OperationMsg { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string EventCreateDate { get; set; }
    }
}
