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
        public Account Login(string userName, string password)
        {
            return _entity.Account.AsNoTracking().FirstOrDefault(a => a.UserName == userName && a.Password == password);
        }
        public List<string > GetApiList(int id)
        {
            return (from ar in _entity.AccountRole.AsNoTracking()
                    join rapi in _entity.RoleApi.AsNoTracking() on ar.RoleID equals rapi.RoleID
                    join api in _entity.ApiCollection.AsNoTracking() on rapi.ApiID equals api.ID
                    select api.ApiName).ToList();
        }
    }
}
