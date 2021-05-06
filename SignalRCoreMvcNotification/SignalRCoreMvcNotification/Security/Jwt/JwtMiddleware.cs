using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SignalRCoreMvcNotification.DataContext;
using SignalRCoreMvcNotification.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Security.Jwt
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<JwtMiddleware> _logger;
        private IConfiguration Configuration { get; }

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            Configuration = configuration;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, SignalRCoreDataContext _dataContext)
        {
            var token = context.Request.Cookies["token"];
            if (token != null)
                attachUserToContext(context, _dataContext, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, SignalRCoreDataContext _dataContext, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Configuration.GetSection("SecretjwtOptions:Secret").Value);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                var userValue = _dataContext.User.Find(userId);
                context.Items["User"] = new UserContextModel(userId, userValue.Username);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
