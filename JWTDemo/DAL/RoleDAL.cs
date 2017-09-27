using JWTDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.DAL
{
    public class RoleDAL
    {
        private readonly BaseEntities _entity;

        public RoleDAL(BaseEntities entity)
        {
            _entity = entity;
        }
        /// <summary>
        /// create new role
        /// </summary>
        /// <param name="insert"></param>
        /// <returns>new role id</returns>
        public int Create(string newRoleName)
        {
            try
            {
                var insert = new Role
                {
                    RoleName = newRoleName
                };
                _entity.Role.Add(insert);
                _entity.SaveChanges();
                return insert.ID;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
