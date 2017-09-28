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
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTDemo.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : BaseController
    {
        private RoleDAL _roleDAL;
        private ILogger<RoleController> _logger;

        public RoleController(BaseEntities db, IOptions<TokenProviderOptions> options, ILogger<RoleController> logger) : base(db, options, logger)
        {
            _roleDAL = new RoleDAL(db);
            _logger = logger;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpPost("Create")]
        [HasPermission("CreateRole")]
        [Consumes("application/json")]
        public IActionResult Create([FromBody] RoleCreateRequestModel insert)
        {
            return Ok(_roleDAL.Create(insert.RoleName));
        }
    }
}
