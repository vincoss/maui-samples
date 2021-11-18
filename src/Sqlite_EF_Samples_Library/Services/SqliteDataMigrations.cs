
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
        private readonly IDatabaseService _databaseService;

        public SqliteDataMigrations(IDatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        public void Run()
        {
            var c = _databaseService.ConnectionString;
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
