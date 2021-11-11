using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sqlite_EF_Samples_Library.Entities
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .Build();

            var builder = new DbContextOptionsBuilder();

            var p = Path.Combine(AppContext.BaseDirectory, $"{nameof(DatabaseContextFactory)}.db3");
            var connectionString = $"Data Source={p};Password=Pass@word1;";

            builder.UseSqlite(connectionString,
                        x => x.MigrationsAssembly(typeof(DatabaseContextFactory).Assembly.FullName));

            return new DatabaseContext(connectionString);
        }
    }
}
