using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JWTDemo.Data;
using JWTDemo.JWTHepler;
using JWTDemo.JwtMiddleware;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTDemo.Controllers
{
   
    public class BaseController : Controller
    {
        public BaseEntities _db;
        private TokenProviderOptions _options;

        public BaseController(BaseEntities db, IOptions<TokenProviderOptions> options)
        {
            _db = db;
            _options = options.Value;
        }
       
    }
}
