using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BlogWebsite.Middlewares
{
    public class JwtCookieMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtCookieMiddleware(RequestDelegate next, TokenValidationParameters tokenValidationParameters)
        {
            _next = next;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["jwt"];

            if (!string.IsNullOrEmpty(token))
            {
                //// Giải mã và xác thực token
                //var handler = new JwtSecurityTokenHandler();
                //var claimsPrincipal = handler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);

                // Thiết lập ClaimsPrincipal vào HttpContext.User để có thể truy cập trong ứng dụng
                //context.User = claimsPrincipal;

                context.Request.Headers.Add("Authorization", "Bearer " + token);
            }

            await _next(context);
        }
    }
}
