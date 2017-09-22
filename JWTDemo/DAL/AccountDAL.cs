using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTDemo.Data;
using Microsoft.EntityFrameworkCore;

namespace JWTDemo.DAL
{
    public class AccountDAL
    {
        private readonly BaseEntities _entity;
        public AccountDAL(BaseEntities entity)
        {
            _entity = entity;
        }
        public List<Account> GetList()
        {
            return _entity.Account.AsNoTracking().ToList();

        }
    }
}
