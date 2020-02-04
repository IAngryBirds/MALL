namespace ILBLI.SnowFlak
{
    public class SnowFlakManager : ISnowFlak
    {
        private const long WorkerID = 1;
        private const long DatacenterID = 1;
        private SnowFlakWorker Worker { get; }

        //IOC注入，使用 AddSingleton进行单例注入，这里就可以不需要使用懒加载模式
        public SnowFlakManager()
        {
            Worker = new SnowFlakWorker(WorkerID, DatacenterID);
        }
         
        /// <summary>
        /// 获取ID号
        /// </summary>
        /// <returns></returns>
        public long GetNextID()
        {
            return Worker.NextID();
        }

    }
}
