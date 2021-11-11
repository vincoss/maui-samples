
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sqlite_EF_Samples_Library.Entities;
using Sqlite_EF_Samples_Library.Interfaces;
using System;


namespace Sqlite_EF_Samples_Library.Services
{
    public class SqliteDataMigrations : IDataMigrations
    {
        private readonly IPath _path;

        public SqliteDataMigrations(IPath path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public void Run()
        {
            var p = _path.GetDatabasePath($"{nameof(SqliteDataMigrations)}.db3");
            var c = $"Data Source={p};Password=Pass@word1;";
            EnsureDatabase(c);
            RunMigrations(c);
        }

        private void RunMigrations(string connectionString)
        {
            try
            {
                using var db = new DatabaseContext(connectionString);
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error executing migration!", ex);
            }
        }

        private static void EnsureDatabase(string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open(); // Will create a database file
            }
        }
    }
}
