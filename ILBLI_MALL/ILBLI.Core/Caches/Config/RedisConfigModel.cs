using System.Collections.Generic;

namespace ILBLI.Core
{
    public class RedisConfigModel
    {
        public string RedisConfigCode { get; set; }
        public List<RedisConnectionModel> RedisMasterList { get; set; }
        public List<RedisConnectionModel> RedisSlaveList { get; set; }
        public int MaxReadPoolSize { get; set; }
        public int MaxWritePoolSize { get; set; }
        public bool AutoStart { get; set; }

    }

    public class RedisConnectionModel
    {
        public string RedisAddress { get; set; }
        public string RedisPort { get; set; }
        public string RedisPassWord { get; set; }
    }
}
