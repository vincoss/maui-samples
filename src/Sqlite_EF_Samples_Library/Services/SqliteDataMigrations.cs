
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sqlite_EF_Samples_Library.Entities;
using Sqlite_EF_Samples_Library.Entities.Model;
using Sqlite_EF_Samples_Library.Interfaces;
using System;
using System.IO;

namespace Sqlite_EF_Samples_Library.Services
{
    public class SqliteDataMigrations : IDataMigrations
    {
        private readonly IDatabaseService _databaseService;

        public SqliteDataMigrations(IDatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        public bool IsMigrated()
        {
            var c = _databaseService.ConnectionString;
            return File.Exists(_databaseService.DatabasePath) && IsMigrationRequired(c);
        }

        public void Run()
        {
            var c = _databaseService.ConnectionString;

            if (IsMigrated())
            {
                return;
            }

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

        private static bool IsMigrationRequired(string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var sql = $"SELECT 1 FROM sqlite_master WHERE type='table' AND name='{nameof(Item)}'";
                var cmd = new SqliteCommand(sql, connection);
                var result = (long?)cmd.ExecuteScalar();

                if (result == null)
                {
                    return false;
                }
                return result == 1;
            }
        }
    }
}
