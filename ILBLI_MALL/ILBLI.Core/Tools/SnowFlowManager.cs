using SnowFlake;
using System;

namespace ILBLI.Core
{
    /// <summary>
    /// 雪花算法--获取唯一ID
    /// </summary>
    public class SnowFlowManager
    {
        private const long WorkerID = 1;
        private const long DatacenterID = 1;
        public IdWorker Worker { get; }

        private SnowFlowManager() {
            Worker = new IdWorker(WorkerID, DatacenterID);
        }

        private static Lazy<SnowFlowManager> lazy = new Lazy<SnowFlowManager>(() =>{ return new SnowFlowManager(); });

        public static SnowFlowManager Instance
        {
            get{ return lazy.Value; }
        }

        /// <summary>
        /// 获取ID号
        /// </summary>
        /// <returns></returns>
        public long GetNextID()
        {
            return Worker.NextId();
        }

    }
}
