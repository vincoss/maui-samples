using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sqlite_Dapper_Samples.Interfaces;
using System;


namespace Sqlite_Dapper_Samples.Services
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
            var serviceProvider = new ServiceCollection()
          //  .AddLogging(lb => lb.AddDebug().AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(
                builder => builder
                    .AddSQLite()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(SqliteDataMigrations).Assembly).For.Migrations())
            .Configure<ProcessorOptions>(options =>
            {
                options.Timeout = TimeSpan.FromSeconds(90);
            })
            .Configure<SelectingProcessorAccessorOptions>(options =>
            {
                options.ProcessorId = "sqlite";
            })
            .BuildServiceProvider(false);

            try
            {
                using var scope = serviceProvider.CreateScope();
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error executing migration!", ex);
            }
        }

        private static void EnsureDatabase(string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            //using (var connection = new System.Data.SQLite.SQLiteConnection(connectionString))
            {
                connection.Open(); // Will create a database file
            }
        }
    }
}
