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
using Microsoft.Extensions.Logging;

namespace JWTDemo.JwtMiddleware
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private TokenProviderOptions _options;
        private JwtSetting _setting;
        private BaseEntities _db;
        private AccountDAL _accDal;
        private JwtHelper _jwtHelper;
        ILogger<TokenProviderMiddleware> _logger;

        public TokenProviderMiddleware(RequestDelegate next, IOptions<TokenProviderOptions> options, IOptions<JwtSetting> setting, ILogger<TokenProviderMiddleware> logger)
        {
            _next = next;
            _options = options.Value;
            _setting = setting.Value;
            _logger = logger;
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

                _jwtHelper = new JwtHelper(context, _accDal, _setting);
                var status = _jwtHelper.VaildateToken();
                switch (status)
                {
                    case Models.JWTStatus.Invalid:
                        context.Response.StatusCode = 400;
                        _logger.LogWarning(LoggingEvents.BadRequest, "Validate token");
                        return context.Response.WriteAsync("Bad Request");
                    case Models.JWTStatus.Timeout:
                        _logger.LogWarning(LoggingEvents.Timeout, "Token Timeout");
                        return RefreshToken(context);
                    default:
                        _logger.LogInformation("Valid Token");
                        return _next(context);
                }

            }
            else// if (!context.Request.Method.Equals("POST") || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad Request");
            }
            //   return GenerateToken(context);
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
