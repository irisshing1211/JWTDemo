using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.Data
{
    public class DbInitializer
    {
        public static void Initialize(BaseEntities context)
        {
            context.Database.EnsureCreated();

            if (context.Account.Any()) return;

            Role adminRole = new Role { RoleName = "admin" };
            Account ac = new Account {  Password = "12345", UserName = "admin", CreateDate = DateTime.Now, Email = "email@email.com", Tel = "123456789" };
            AccountRole accRole = new AccountRole { RoleID = 1, AccountID = 1 };
            List<ApiCollection> apis = new List<ApiCollection>() {
                new ApiCollection{ ApiName="GetAllAccount", Path="/api/Account"},
                new ApiCollection{ApiName="CreateRole", Path="/api/Role/Create"},
                new ApiCollection{  ApiName="CreateAccount", Path="/api/Account/Post"}
            };
            List<RoleApi> roleapis = new List<RoleApi>()
            {
                new RoleApi{ ApiID=1, RoleID=1},
                new RoleApi{ApiID=2, RoleID=1},
                new RoleApi{ ApiID=3, RoleID=1}
            };

            context.Account.Add(ac);
            context.Role.Add(adminRole);
            context.ApiCollection.AddRange(apis);
            context.SaveChanges();

            context.RoleApi.AddRange(roleapis);
            context.AccountRole.AddRange(accRole);
            context.SaveChanges();
        }
    }
}
