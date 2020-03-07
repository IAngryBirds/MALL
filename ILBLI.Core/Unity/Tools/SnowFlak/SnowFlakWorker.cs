using System;
using System.Collections.Generic;
using System.Text;

namespace ILBLI.SnowFlak
{
    public class SnowFlakWorker
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

        private long mLastTimestamp = -1L;

        public long WorkerId { get; protected set; }
        public long DatacenterId { get; protected set; }
        public long Sequence { get; internal set; } = 0L;

        private static readonly object mLock = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workerId">当前机器标识码</param>
        /// <param name="datacenterId">当前数据中心标识码【可以自己配，也可以取自系统进程ID】</param>
        /// <param name="sequence"></param>
        public SnowFlakWorker(long workerId, long datacenterId, long sequence = 0L)
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            Sequence = sequence;

            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new ArgumentException($"worker Id can't be greater than {MaxWorkerId} or less than 0");
            } 
            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException($"datacenter Id can't be greater than {MaxDatacenterId} or less than 0");
            }
        }
        
        public virtual long NextID()
        {
            lock (mLock)
            {
                var timestamp = TimeGen();

                if (timestamp < mLastTimestamp)
                {
                    throw new Exception($"Clock moved backwards.  Refusing to generate id for {mLastTimestamp - timestamp} milliseconds");
                }

                if (mLastTimestamp == timestamp)
                {
                    Sequence = (Sequence + 1) & SequenceMask;
                    if (Sequence == 0)
                    {
                        timestamp = TilNextMillis(mLastTimestamp);
                    }
                }
                else
                {
                    Sequence = 0;
                }

                mLastTimestamp = timestamp;
                //将所有的数字转换位2进制参与“与”运算，转换回10进制输出  
                // <<有符号左移，将运算数的二进制整体左移指定位数，低位0补齐，也就是1001 ==》转成2进制1111101001==》左移11111010010000000000==》转为十进制1025024
                var id = ((timestamp - Twepoch) << TimestampLeftShift) |
                         (DatacenterId << DatacenterIdShift) |
                         (WorkerId << WorkerIdShift) | Sequence;
                return id;
            }
        }

        /// <summary>
        /// 获取有效的时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            //当前时间戳小于记录的最后一次时间戳，则一直获取最新的时间戳，并循环判断，直到大于最后一次时间戳为止
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        protected virtual long TimeGen()
        {
            return System.CurrentTimeMillis();
        }
    }
}
