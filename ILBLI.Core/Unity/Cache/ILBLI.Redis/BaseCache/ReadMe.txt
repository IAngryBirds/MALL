配置信息：
"BaseCache": {
    "InstanceName": "ilbli",//实例名
    "Configuration": "127.0.0.1:6379"//链接地址
  },
  "JsonCacheConfig": {//缓存种类，及其缓存时间设置
    "JsonCaches": [
      {
        "CacheType": "ModuleType",
        "CacheStr": "Global_ModuleType",
        "CacheTime": "10"
      },
      {
        "CacheType": "UserFriendCache",
        "CacheStr": "UserFriendCache_[UserAccount]",
        "CacheTime": "30"
      }
    ]
  }

1.  添加基础的Redis缓存操作 ： 
    1.1 在StartUp中注入AddBaseRedisCache即可；
2.  使用方式：
    2.1 在构造函数中注入该IBaseCache，
        private readonly ILogger _Logger =null; 
        private readonly ILogoService _Logo = null;
        private readonly IBaseCache _BaseCache = null;
        private readonly RedisStringService _StringService = null;
        public HomeController(ILogger<HomeController> logger,ILogoService logo,IBaseCache cache)
        {
            _Logger = logger; 
            _Logo = logo;
            _BaseCache = cache; 
        }
    2.2 在使用的地方调用相关方法即可
        [HttpGet]
        public TResult WriteAndGetCache(string cacheStr)
        {
           
            bool isExist = this._BaseCache.Exist("a");
            if (!isExist)
            { 
                this._BaseCache.Set("a", cacheStr, new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddMinutes(1)));
            }
            var msg = this._BaseCache.Get<string>("a", out isExist);
            if(isExist)
                return TResult.DataSuccess(msg);
            else
                return TResult.Fail();
        }
