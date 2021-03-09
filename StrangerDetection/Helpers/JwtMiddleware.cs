using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StrangerDetection.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrangerDetection.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate __next;
        private readonly AppSetting _appSetting;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSetting> appSetting) 
        {
            __next = next;
            _appSetting = appSetting.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context
                .Request
                .Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                attachUserToContext(context, userService, token);
            }

           await __next(context);
        }

        private void attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSetting.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken) ;
                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(x=> x.Type == "username").Value;
                context.Items["User"] = userService.GetByUsername(username);
            }
            catch
            {

            }
        }
    }
}
