using System;

namespace SnowFlake
{
    public class IdWorker
    {
        public const long Twepoch = 1288834974657L;

        const int WorkerIdBits = 5;//机器标识位数
        const int DatacenterIdBits = 5;//数据中心ID位数
        const int SequenceBits = 12;//自增位数
        const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);//机器标识位的最大值
        const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);//数据中心ID位数的最大值

        private const int WorkerIdShift = SequenceBits;
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);

        private long _lastTimestamp = -1L;

        public long WorkerId { get; protected set; }
        public long DatacenterId { get; protected set; }
        public long Sequence { get; internal set; } = 0L;

        readonly object _lock = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workerId">当前机器标识码</param>
        /// <param name="datacenterId">当前数据中心标识码【可以自己配，也可以取自系统进程ID】</param>
        /// <param name="sequence"></param>
        public IdWorker(long workerId, long datacenterId, long sequence = 0L)
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            Sequence = sequence;

            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new ArgumentException(string.Format("worker Id can't be greater than {0} or less than 0", MaxWorkerId));
            }

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException(string.Format("datacenter Id can't be greater than {0} or less than 0", MaxDatacenterId));
            }
        }




        public virtual long NextId()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();

                if (timestamp < _lastTimestamp)
                {
                    throw new Exception(string.Format(
                        "Clock moved backwards.  Refusing to generate id for {0} milliseconds", _lastTimestamp - timestamp));
                }

                if (_lastTimestamp == timestamp)
                {
                    Sequence = (Sequence + 1) & SequenceMask;
                    if (Sequence == 0)
                    {
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    Sequence = 0;
                }

                _lastTimestamp = timestamp;
                //将所有的数字转换位2进制参与“与”运算，转换回10进制输出  
                // <<有符号左移，将运算数的二进制整体左移指定位数，低位0补齐，也就是1001 ==》转成2进制1111101001==》左移11111010010000000000==》转为十进制1025024
                var id = ((timestamp - Twepoch) << TimestampLeftShift) |
                         (DatacenterId << DatacenterIdShift) |
                         (WorkerId << WorkerIdShift) | Sequence;
                return id;
            }
        }

        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        protected virtual long TimeGen()
        {
            return System.CurrentTimeMillis();
        }
    }
}