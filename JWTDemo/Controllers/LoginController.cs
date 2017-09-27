using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JWTDemo.Data;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTDemo.Controllers
{
    public class LoginController : Controller
    {
        public LoginController(BaseEntities db)
        {

        }
        public IActionResult Login()
        {
            return Ok();
        }
    }
}
