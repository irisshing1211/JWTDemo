using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTDemo.Data
{
    public class BaseEntities : DbContext, IDisposable
    {
        public BaseEntities(DbContextOptions<BaseEntities> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountRole> AccountRole { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<ApiCollection> ApiCollection { get; set; }
        public virtual DbSet<RoleApi> RoleApi { get; set; }
    }
}
