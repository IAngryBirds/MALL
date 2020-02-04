using StackExchange.Redis;

namespace ILBLI.Core
{
    /// <summary>
    /// 方式二：通过获取连接上下文类，自己调用相应的操作方法
    /// </summary>
    public class RedisExcFactory
    { 
        private RedisEnum RedisType { get; set; }
        private ConnectionMultiplexer cache { get; set; }

        public RedisExcFactory()
        {
            this.RedisType = RedisEnum.CommonRedis;
            this.cache = RedisExConfig.CreateRedisConnection(RedisType); 
        }

        public RedisExcFactory(RedisEnum redisEnum)
        {
            this.RedisType = redisEnum;
            this.cache = RedisExConfig.CreateRedisConnection(RedisType); 
        }

        /// <summary>
        /// 获取Redis连接类
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDataBase()
        {
            return cache.GetDatabase();
        }
    }
}
