using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JWTDemo.Data;
using JWTDemo.DAL;
using JWTDemo.Models;
using Microsoft.Extensions.Options;
using JWTDemo.JwtMiddleware;
using JWTDemo.JWTHepler;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTDemo.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : BaseController
    {
        private AccountDAL _accDal;
        private JwtHelper _jwtHelper;
        private TokenProviderOptions _options;

        public LoginController(BaseEntities db, IOptions<TokenProviderOptions> options) : base(db, options)
        {
            _accDal = new AccountDAL(db);
            _options = options.Value;
            _jwtHelper = new JwtHelper(_accDal, _options);
        }
        //public LoginController(BaseEntities db)//, IOptions<TokenProviderOptions> options)
        //{
        //    _accDal = new AccountDAL(db);
        //    //_options = options.Value;
        //    //_jwtHelper = new JwtHelper(_accDal, _options);
        //}
        //public LoginController() { }
        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginRequestModel model)
        {
            var acc = _accDal.Login(model.UserName, model.Password);

            if (acc == null)
            {
                return NoContent();
            }
            var now = DateTime.UtcNow;

            //    var apis = _accDal.GetApiList(acc.ID);

            var encodedJwt = _jwtHelper.GenerateToken(acc);//, apis);

            var response = new LoginResponseModel
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds
            };
            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
