using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

namespace JWTDemo.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BaseEntities>
    {
        public BaseEntities CreateDbContext(string[] args)
        {
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            var builder = new DbContextOptionsBuilder<BaseEntities>();

            var connectionString = "Data Source=localhost;Initial Catalog=JWTDemo;Integrated Security=True";//configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new BaseEntities(builder.Options);
        }
    }
}
