using CatalogApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogApi.Helpers
{
    public class JwtMiddleware
    {
        private RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, IJwtUtils jwtUtils)
        {
            _next = next;
            _appSettings = appSettings.Value;
            
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetById(userId.Value);

            await _next(context);
        }
    }
}