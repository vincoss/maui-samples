using Dapper;
using Microsoft.Data.Sqlite;
using Sqlite_Dapper_Samples.Entities.Model;
using Sqlite_Dapper_Samples.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite_Dapper_Samples.Services
{
    public interface IItemService
    {
        Task Create(Item product);
        Task<IEnumerable<Item>> Get();

    }

    public class ItemService : IItemService
    {
        private readonly IDatabaseService _databaseConfig;

        public ItemService(IDatabaseService databaseConfig)
        {
            this._databaseConfig = databaseConfig;
        }

        public async Task Create(Item product)
        {
            using (var connection = new SqliteConnection(_databaseConfig.ConnectionString))
            {
                await connection.ExecuteAsync(@"INSERT INTO Item (Name, Description) VALUES (@Name, @Description);", product);
            }
        }

        public async Task<IEnumerable<Item>> Get()
        {
            using (var connection = new SqliteConnection(_databaseConfig.ConnectionString))
            {
                return await connection.QueryAsync<Item>("SELECT rowid AS Id, Name, Description FROM Item;");
            }
        }
    }
}
