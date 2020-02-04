using ILBLI.IService;
using ILBLI.RabbitMQ;
using ILBLI.Redis;
using ILBLI.SnowFlak;
using ILBLI.Unity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace ILBLI.WebAPI.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class HomeController : ControllerBase
    {
        private readonly ILogger _Logger =null; 
        private readonly ILogoService _Logo = null;
        private readonly IBaseCache _BaseCache = null;
        private readonly RedisStringService _StringService = null;
        private readonly IRabbitManage _RabbitMQ = null;
        public HomeController(ILogger<HomeController> logger,ILogoService logo,IBaseCache cache,RedisStringService stringService,IRabbitManage mq)
        {
            _Logger = logger; 
            _Logo = logo;
            _BaseCache = cache;
            _StringService = stringService;
            _RabbitMQ = mq;
        }

        [HttpGet] 
        public TResult Login(string name,string pwd)
        {
            _Logger.LogInformation("登录中....");

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pwd))
            {
              
                return TResult.Fail("用户名/密码不能为空");
            }
            else
            {
                ClaimsIdentity claimIdentity = new ClaimsIdentity();
                claimIdentity.AddClaim(new Claim(ClaimTypes.Name, name));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Role, "AAA"));
                claimIdentity.AddClaim(new Claim("自定义组策略", "this is my custom policy"));


                claimIdentity.AddClaim(new Claim("UserID", "012345"));
                 
                AuthenticationProperties properties = new AuthenticationProperties();
                  
                properties.GetJwtAuthenticationProperties("ilbli_custom_key", DateTime.UtcNow.AddMinutes(10), claims: claimIdentity.Claims);
                
                string token = properties.GetTokenValue("ILBLI_Token");
                return TResult.DataSuccess(token);
 
            }
        }

        [HttpGet]
        [Authorize(Roles ="AAA,BBB",Policy ="自定义组策略")]
        public void LogOut()
        {
            /*读取HttpContext.User中的用户信息（两种读取方式）*/
            string user = HttpContext.User.FindFirst("UserID")?.Value;
            string userName = HttpContext.User.FindFirst(c=>c.Type== ClaimTypes.Name)?.Value;
            string userRole = HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;
            _Logger.LogInformation($"{user}_{userName}_{userRole}登出中....");
            
        }

        [HttpGet]
        public TResult GetList()
        {
            var lst= _Logo.QueryLogoList();
            return TResult.ListSuccess(lst);
        }

        [HttpGet]
        public TResult Insert()
        {
            var result = _Logo.Insert();
            return TResult.Success();
        }

        [HttpGet]
        public TResult GetUID()
        {
            var uid= _Logo.GetUID();
            return TResult.DataSuccess(uid);
        }

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

        [HttpGet]
        public TResult WriteAndGetCacheMore(string cacheStr)
        {
            string key = "CS";
            if(!_StringService.KeyExists(key))
                _StringService.StringSet(key, cacheStr,new TimeSpan(0,1,0));

            var msg = _StringService.StringGet(key);
            
            return TResult.DataSuccess(msg);
        }

        /// <summary>
        /// 推送消息进消息队列
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        public TResult SendMessageToMQ(string message)
        {
            _RabbitMQ.SendToMQ("OATest", message);

            return TResult.Success();
        }
    }
}