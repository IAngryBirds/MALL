配置信息：
 "BaseCache": { 
    "Configuration": "127.0.0.1:6379"//链接地址
  },
   
1.  添加基础的Redis缓存操作 ： 
    1.1 在StartUp中注入AddRedisServiceCache即可；
2.  使用方式：
    2.1 在构造函数中注入该IBaseCache，
        private readonly ILogger _Logger =null; 
        private readonly ILogoService _Logo = null; 
        private readonly RedisStringService _StringService = null;
        public HomeController(ILogger<HomeController> logger,ILogoService logo,RedisStringService stringService)
        {
            _Logger = logger; 
            _Logo = logo; 
            _StringService = stringService;
        }

    2.2 在使用的地方调用相关方法即可
        [HttpGet]
        public TResult WriteAndGetCacheMore(string cacheStr)
        {
            string key = "CS";
            if(!_StringService.KeyExists(key))
                _StringService.StringSet(key, cacheStr,new TimeSpan(0,1,0));

            var msg = _StringService.StringGet(key);
            
            return TResult.DataSuccess(msg);
        }
