using JWTDemo.DAL;
using JWTDemo.Data;
using JWTDemo.JWTHepler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using JWTDemo.JwtMiddleware;

namespace JWTDemo.JwtMiddleware
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private TokenProviderOptions _options;
        private BaseEntities _db;
        private AccountDAL _accDal;
        private JwtHelper _jwtHelper;

        public TokenProviderMiddleware(RequestDelegate next, IOptions<TokenProviderOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public Task Invoke(HttpContext context, BaseEntities db)
        {
            _db = db;
            _accDal = new AccountDAL(db);
           
            //if path == login & is post -> gen token
            if (context.Request.Path.Equals(_options.Path, StringComparison.Ordinal) &&
                context.Request.Method.Equals("POST"))
            {
                return _next(context);
                //_jwtHelper = new JwtHelper(_accDal, _options);
                //return GenerateToken(context);
            }
            //if path != login & has Authorization & Authorization start with Bearer -> valid token 
            else if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal) &&
                !string.IsNullOrEmpty(context.Request.Headers["Authorization"]) && context.Request.Headers["Authorization"].ToString().StartsWith("Bearer"))
            {

                _jwtHelper = new JwtHelper(context, _accDal, _options);
                var status = _jwtHelper.VaildateToken();
                switch (status)
                {
                    case Models.JWTStatus.Invalid:
                        context.Response.StatusCode = 400;
                        return context.Response.WriteAsync("Bad Request");
                    case Models.JWTStatus.Timeout:
                        return RefreshToken(context);
                    default:
                        return _next(context);
                }
                
            }
            if (!context.Request.Method.Equals("POST") || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad Request");
            }
            return GenerateToken(context);
        }
        private async Task GenerateToken(HttpContext context)
        {
            try
            {
                string username = context.Request.Form["UserName"];
                string password = context.Request.Form["Password"];

                var acc = _accDal.Login(username, password);

                if (acc == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid username or password");
                    return;
                }
                var now = DateTime.UtcNow;

                var apis = _accDal.GetApiList(acc.ID);

                //    List<Claim> claims = new List<Claim>()
                //{
                //    new Claim(ClaimTypes.NameIdentifier, acc.ID.ToString()),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                //};

                //    foreach (var x in apis)
                //    {
                //        claims.Add(new Claim(ClaimTypes.Role, x));
                //    }


                //    var jwt = new JwtSecurityToken(
                //        issuer: _options.Issuer,
                //        audience: _options.Audience,
                //        claims: claims,
                //        notBefore: now,
                //        expires: now.Add(_options.Expiration),
                //        signingCredentials: _options.SigningCredentials);

                //    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var encodedJwt = _jwtHelper.GenerateToken(acc, apis);

                var response = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)_options.Expiration.TotalSeconds
                };

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response,
                    new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented
                    }));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"Exception {ex.Message}");
                return;
            }
        }

        private async Task RefreshToken(HttpContext context)
        {

        }
    }
    public static class TokenProviderMiddlewareExtensions
    {
        public static IApplicationBuilder UseJWTTokenProviderMiddleware(this IApplicationBuilder builder, IOptions<TokenProviderOptions> options)
        {
            return builder.UseMiddleware<TokenProviderMiddleware>(options);
        }
    }
}
