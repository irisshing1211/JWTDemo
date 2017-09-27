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

        private List<string> GetApiList(int id)
        {
            return (from ar in _entity.AccountRole.AsNoTracking()
                    join rapi in _entity.RoleApi.AsNoTracking() on ar.RoleID equals rapi.RoleID
                    join api in _entity.ApiCollection.AsNoTracking() on rapi.ApiID equals api.ID
                    where ar.AccountID == id
                    select api.ApiName).ToList();
        }
        public Account Login(string userName, string password)
        {
            return _entity.Account.AsNoTracking().FirstOrDefault(a => a.UserName == userName && a.Password == password);
        }
        public Account Get(string userName)
        {
            return _entity.Account.FirstOrDefault(a => a.UserName == userName);
        }
        public bool CheckPermission(int accid, string apiName)
        {
            return (from ar in _entity.AccountRole.AsNoTracking()
                    join rapi in _entity.RoleApi.AsNoTracking() on ar.RoleID equals rapi.RoleID
                    join api in _entity.ApiCollection.AsNoTracking() on rapi.ApiID equals api.ID
                    where ar.AccountID == accid && api.ApiName == apiName
                    select api).Count() > 0;
            //var list = GetApiList(accid);
            //return list.Contains(api);
        }
    }
}
