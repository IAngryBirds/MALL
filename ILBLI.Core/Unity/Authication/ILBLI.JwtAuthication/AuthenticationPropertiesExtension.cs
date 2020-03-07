using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ILBLI.Unity
{
    public static class AuthenticationPropertiesExtension
    {
        /// <summary>
        /// 进行Jwt认证（生成token）
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="serurityKey">密钥（不得为空）</param>
        /// <param name="expires">过期时间</param>
        /// <param name="tokenKeyName">生成token的名称</param>
        /// <param name="claims">用户信息</param>
        /// <param name="issuer">发行方</param>
        /// <param name="audience">发行方集合</param>
        /// <returns></returns>
        public static AuthenticationProperties GetJwtAuthenticationProperties(this AuthenticationProperties properties, string serurityKey, DateTime expires, string tokenKeyName = "ILBLI_Token", IEnumerable<Claim> claims = null, string issuer = "ilbli.com", string audience = "ilbli.com")
        {
            if (string.IsNullOrWhiteSpace(serurityKey))
            {
                throw new ArgumentNullException("Jwt密钥", "Jwt密钥尚未初始化");
            }

            //使用密钥对令牌进行签名。这个密钥将在您的API和需要检查令牌是否合法的任何系统之间共享。
            //这里的Key密钥可以给不同的三方系统颁发的密（公）钥，然后进行加密-生成token，解密的时候也根据不同的系统的密钥进行解密
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(serurityKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: issuer, //token证书颁发中心（网址）如果签发的时候这个claim的值是“a.com”，验证的时候如果这个claim的值不是“a.com”就属于验证失败
                audience: audience,//如果签发的时候这个claim的值是“[‘b.com’,’c.com’]”，验证的时候这个claim的值至少要包含 b.com，c.com 的其中一个才能验证通过；
                claims: claims,
                notBefore: DateTime.UtcNow,//如果验证的时候小于这个claim指定的时间，就属于验证失败； 
                expires: expires,//如果验证的时候超过了这个claim指定的时间，就属于验证失败
                signingCredentials: creds //签发的密钥
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            List<AuthenticationToken> tokenAuths = new List<AuthenticationToken>();
            tokenAuths.Add(
                new AuthenticationToken()
                {
                    Name = tokenKeyName,
                    Value = token
                });

            properties.StoreTokens(tokenAuths);
            return properties;
        }



    }
}
