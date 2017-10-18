using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using JWTDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using JWTDemo.JwtMiddleware;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

namespace JWTDemo
{
    public class Startup
    {
        private static string secretKey, issuer, audience;// "mysupersecret_secretkey!123";
        private static SymmetricSecurityKey signingKey;

        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //    signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        //}

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

        }
        // public IConfiguration Configuration { get; }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            secretKey = Configuration.GetSection("JwtSetting").GetSection("Key").Value;
            audience = Configuration.GetSection("JwtSetting").GetSection("audience").Value;
            issuer = Configuration.GetSection("JwtSetting").GetSection("issuer").Value;

            signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            // Add framework services.
            services.AddDbContext<BaseEntities>(
                e =>
                {
                    e.EnableSensitiveDataLogging();
                    e.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });
            services.AddMvc();

            var tokenValidationParameters = new TokenValidationParameters
            {
                //The signing key must match !
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                //Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = issuer,

                //validate the JWT Audience (aud) claim

                ValidateAudience = true,
                ValidAudience = audience,

                //validate the token expiry
                ValidateLifetime = true,

                // If you  want to allow a certain amout of clock drift
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });

            services.Configure<JwtSetting>(o => Configuration.GetSection("JwtSetting").Bind(o));

            //    services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            //  services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)//, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            // app.UseJwtBearerAuthentication();
            app.UseAuthentication();

            var jwtOptions = new TokenProviderOptions
            {
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                Key = secretKey,
                Expiration = int.Parse(Configuration.GetSection("JwtSetting").GetSection("Expire").Value.ToString())
            };
            app.UseJWTTokenProviderMiddleware(Options.Create(jwtOptions));

            app.UseMvc();


        }
    }
  
}
