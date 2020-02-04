using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;

namespace ILBLI.Unity
{
    /// <summary>
    /// 半成品，无法组成完成业务链，故暂时性放弃
    /// </summary>
    public static  class UseAuthenticationInit
    {
        public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder app)
        {
            app.Map("/login", builder => builder.Use(next =>
            {
                return async (context) =>
                
                {
                    string name= context.Request.Query["name"];
                    string pwd = context.Request.Query["pwd"];
                    if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pwd))
                    {
                        context.Response.ContentType = "text/plain;charset=utf-8";
                        await context.Response.WriteAsync("用户名/密码不能为空");
                    }
                    else
                    { 
                        var claimIdentity = new ClaimsIdentity();
                        claimIdentity.AddClaim(new Claim(ClaimTypes.Name, name));
                        
                        AuthenticationProperties properties = new AuthenticationProperties();
                        //properties.GetJwtAuthenticationProperties("ilbli_custom_key", DateTime.Now.AddMinutes(10), claims: claimIdentity.Claims);

                        await context.SignInAsync(typeof(ILBLISchemeHandler).Name, new ClaimsPrincipal(claimIdentity), properties);
                         
                    }
                     
                };
            }));

            // 退出
            app.Map("/logout", builder => builder.Use(next =>
            {
                return async (context) =>
                {
                    await context.SignOutAsync(typeof(ILBLISchemeHandler).Name);
                };
            }));

            // 认证
            app.Use(next =>
            {
                return async (context) =>
                {
                    var result = await context.AuthenticateAsync(typeof(ILBLISchemeHandler).Name);
                    if (result?.Principal != null) 
                        context.User = result.Principal;
                    
                    await next(context);
                };
            });

            // 授权
            app.Use(async (context, next) =>
            {
                var user = context.User;
                if (user?.Identity?.IsAuthenticated ?? false)
                {
                    if (user.Identity.Name != "jim")
                        await context.ForbidAsync(typeof(ILBLISchemeHandler).Name);
                    else
                        await next();
                }
                else
                {
                    await context.ChallengeAsync(typeof(ILBLISchemeHandler).Name);
                }
            });


            return app;
        }
    }
}
