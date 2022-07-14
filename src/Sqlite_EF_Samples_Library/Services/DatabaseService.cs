using Sqlite_EF_Samples_Library.Interfaces;
using System;


namespace Sqlite_EF_Samples_Library.Services
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
                var c = $"Data Source={DatabasePath};Password=Pass@word1;";
                return c;
            }
        }

        public string DatabasePath => _path.GetDatabasePath(DatabaseName);
    }
}
