using Sqlite_EF_Samples_Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite_EF_Samples_Library.Services
{
    public class Bootstrap : IBootstrap
    {
        private readonly IDataMigrations _dataMigrations;

        public Bootstrap(IDataMigrations dataMigrations)
        {
            _dataMigrations = dataMigrations ?? throw new ArgumentNullException(nameof(dataMigrations));
        }

        public void Run()
        {
            _dataMigrations.Run();
        }
    }
}
