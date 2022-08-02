using Microsoft.EntityFrameworkCore;
using Sqlite_EF_Samples_Library.Entities.Configuration;
using Sqlite_EF_Samples_Library.Entities.Model;
using System;
using System.Data.Common;


namespace Sqlite_EF_Samples_Library.Entities
{
    public class DatabaseContext : DbContext
    {
        private readonly DbConnection _connection;

        public DatabaseContext(DbConnection dbConnection)
        {
            _connection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ItemConfiguration());
        }
    }
}
