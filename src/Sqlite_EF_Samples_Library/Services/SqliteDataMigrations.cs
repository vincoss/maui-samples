
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sqlite_EF_Samples_Library.Entities;
using Sqlite_EF_Samples_Library.Entities.Model;
using Sqlite_EF_Samples_Library.Interfaces;
using System;
using System.Data.Common;
using System.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Sqlite_EF_Samples_Library.Services
{
    public class SqliteDataMigrations : IDataMigrations
    {
        private readonly DbConnection _connection;
        private readonly IDatabaseService _databaseService;

        public SqliteDataMigrations(IDatabaseService databaseService, DbConnection dbConnection)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _connection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
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
            RunMigrations();
        }

        private void RunMigrations()
        {
            try
            {
                using var db = new DatabaseContext(_connection);
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

                var sql = @"PRAGMA auto_vacuum=FULL;
                            PRAGMA synchronous=normal;
                            PRAGMA temp_store=memory;
                            PRAGMA mmap_size=30000000000;
                            PRAGMA page_size=32768;";

                var cmd = new SqliteCommand(sql, connection);
                cmd.ExecuteNonQuery();
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
