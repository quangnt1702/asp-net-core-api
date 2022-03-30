using System.Linq;
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
            if (token != null) context.Items["accessToken"] = token.Trim();
            await _next(context);
        }

    }
}