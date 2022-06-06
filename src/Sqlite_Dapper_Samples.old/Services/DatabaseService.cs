using Sqlite_Dapper_Samples.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite_Dapper_Samples.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IPath _path;

        public DatabaseService(IPath path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public string DatabaseName => "Database.db3";

        public string ConnectionString
        {
            get
            {
                var p = _path.GetDatabasePath(DatabaseName);
                var c = $"Data Source={p};Password=Pass@word1;";
                return c;
            }
        }
    }
}
