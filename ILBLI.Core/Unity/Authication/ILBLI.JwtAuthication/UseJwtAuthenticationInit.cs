using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ILBLI.Unity
{
    public static class UseJwtAuthenticationInit
    {
        /// <summary>
        /// 注册JWT权限认证服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serurityKey"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string serurityKey, string issuer = "ilbli.com", string audience = "ilbli.com")
        {
            //添加jwt验证：
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey

                        ValidAudience = audience,//Audience
                        ValidIssuer = issuer,//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(serurityKey)),//拿到SecurityKey

                        // RequireSignedTokens = true,
                        // SaveSigninToken = false,
                        // ValidateActor = false,
                        // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                        // ValidateAudience = true,
                        // ValidateIssuer = true, 
                        // ValidateIssuerSigningKey = false,
                        // 是否要求Token的Claims中必须包含Expires
                        // RequireExpirationTime = true,
                        // 允许的服务器时间偏移量
                        // ClockSkew = TimeSpan.FromSeconds(300),
                        // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        // ValidateLifetime = true
                    };
                });
            /*
             * 如果需要在方法中使用 [Authorize(Policy ="自定义组策略")]进行授权控制，那么就需要在这里添加自定义的组策略，
             * 然后在给用户创建身份的时候需要 claimIdentity.AddClaim(new Claim("自定义组策略", "this is my custom policy"));添加这个组策略
             */
            services.AddAuthorization(options =>
                    {
                        options.AddPolicy("自定义组策略", policy => policy.RequireAssertion(context => context.User.HasClaim("自定义组策略", "this is my custom policy")));
                    });

            return services;
        }

        /// <summary>
        /// 请求管道中加入认证+授权中间件
        /// @注册认证中间件 （必须先认证，在授权，顺序反了就BUG了）（也可以两个中只使用一个那么使用）
        /// @并且必须使用在  app.UseRouting()和 app.UseEndpoints()之间
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthenticationThenAuthorization(this IApplicationBuilder app)
        { 
            app.UseAuthentication();//先认证
            app.UseAuthorization();//在授权

            return app;
        }
    }
}
