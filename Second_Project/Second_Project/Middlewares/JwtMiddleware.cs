using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Second_Project.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

      
            if (token != null)
            {
                // var handler = new JwtSecurityTokenHandler();
                // var tokenRead = handler.ReadJwtToken(token);
                // Claim claim = tokenRead.Claims.SingleOrDefault(c => c.Type == "UserName");
                // context.Items["UserName"] = claim.Value;

                context.Items["accessToken"] = token;
            }

            await _next(context);
        }
    }
}