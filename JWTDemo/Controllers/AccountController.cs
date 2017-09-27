using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JWTDemo.Data;
using JWTDemo.DAL;
using JWTDemo.Models;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTDemo.Controllers
{
    [Route("api/[controller]")]
   // [Authorize]
    public class AccountController : Controller
    {
        private readonly BaseEntities _entity;
        private AccountDAL _accountDal;

        public AccountController(BaseEntities entity)
        {
            _entity = entity;
            _accountDal = new AccountDAL(entity);
        }

        // GET: api/values
        [HttpGet]
        [Produces(typeof(List<Account>))]
        [HasPermission("GetAllAccount")]
        public IEnumerable<Account> Get()
        {
            return _accountDal.GetList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("Create")]
        [HasPermission("CreateAccount")]
        public IActionResult Create([FromBody]NewAccountResponseModels responseModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                //using (var scope = new TransactionScope())
                //{
                    Account newAccount = new Account
                    {
                        CreateDate = DateTime.Now,
                        Email = responseModel.Email,
                        Password = responseModel.Password,
                        Tel = responseModel.Tel,
                        UserName = responseModel.UserName
                    };
                    _entity.Account.Add(newAccount);
                    _entity.SaveChanges();

                    AccountRole newAccRole = new AccountRole
                    {
                        AccountID = newAccount.ID,
                        RoleID = responseModel.RoleID
                    };
                    _entity.AccountRole.Add(newAccRole);
                    _entity.SaveChanges();

                  //  scope.Complete();
               // }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
    }
}
