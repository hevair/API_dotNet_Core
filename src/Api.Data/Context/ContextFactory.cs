using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {
            //Usado para Criar as Migrações
            // var connectionString = "Server=localhost;Port=33006;DataBase=dbAPI;Uid=root;Pwd=admin1234";
            var connectionString = "Server=.\\SQLEXPRESS;Initial Catalog=dbapi;User Id=sa;Password=91343609;";
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            // optionsBuilder.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));
            optionsBuilder.UseSqlServer(connectionString);
            return new MyContext(optionsBuilder.Options);
        }
    }
}