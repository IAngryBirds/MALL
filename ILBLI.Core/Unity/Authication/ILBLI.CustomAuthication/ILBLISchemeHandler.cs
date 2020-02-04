using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ILBLI.Unity
{
    /// <summary>
    /// 半成品，无法组成完成业务链，故暂时性放弃
    /// </summary>
    public class ILBLISchemeHandler : IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        public AuthenticationScheme Scheme { get;private set; }
        protected HttpContext Context { get; private set; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            Scheme = scheme;
            Context = context;
            return Task.CompletedTask;
        }
        /// <summary>
        /// 登陆操作 [登陆成功后，向用户颁发一个证书]
        /// </summary>
        /// <param name="user"></param>
        /// <param name="properties">表示证书颁发的相关信息</param>
        /// <returns></returns>
        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        { 
            //将Token写入数据库/缓存 
            string token = properties?.GetTokenValue("ILBLI_Token");
            if (!string.IsNullOrWhiteSpace(token)) 
                ILBLI_Dictionary.ILBLI_Dic[token]=user;

            Context.Response.WriteAsync(token);

            return Task.CompletedTask;
        }
        /// <summary>
        /// 注销操作
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignOutAsync(AuthenticationProperties properties)
        {
            //将Token从数据库/缓存中移除
            string token = properties.GetTokenValue("ILBLI_Token");
            ILBLI_Dictionary.ILBLI_Dic.Remove(token);
            return Task.CompletedTask;
        }
        /// <summary>
        /// 认证 验证在 SignInAsync 中颁发的证书，并返回一个 AuthenticateResult 对象，表示用户的身份
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            string token = string.Empty;
            

             
            if (token == null)
            {
                return AuthenticateResult.Fail(new Exception("未登录/Token已过期"));
            }
            if (ILBLI_Dictionary.ILBLI_Dic.ContainsKey(token))
            {
                return AuthenticateResult.Success(new AuthenticationTicket(Context.User, "ILBLIScheme"));
            }
            return AuthenticateResult.Fail(new Exception("未登录/Token已过期"));
        }
        /// <summary>
        /// 未授权 返回一个需要认证的标识来提示用户登录，通常会返回一个 401 状态码
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
        /// <summary>
        /// 无权限 禁上访问，表示用户权限不足，通常会返回一个 403 状态码。
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ForbidAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }

       
    }
}
