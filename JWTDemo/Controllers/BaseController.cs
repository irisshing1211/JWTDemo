using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JWTDemo.Data;
using JWTDemo.JWTHepler;
using JWTDemo.JwtMiddleware;
using Microsoft.Extensions.Options;
using Serilog;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTDemo.Controllers
{

    public class BaseController : Controller
    {
        public BaseEntities _db;
        private TokenProviderOptions _options;
        ILogger<BaseController> _logger;

        public BaseController(BaseEntities db, IOptions<TokenProviderOptions> options, ILogger<BaseController> logger)
        {
            _db = db;
            _options = options.Value;
            _logger = logger;
        }
        public void Logging(int loggingEvents, string message)//, object obj)
        {
            _logger.LogInformation(LoggingEvents.Location, "{controller}/{action}",
                ControllerContext.ActionDescriptor.ControllerName,
                ControllerContext.ActionDescriptor.ActionName);
        }

    }
}
