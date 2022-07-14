using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite_EF_Samples_Library.Interfaces
{
    public interface IDataMigrations
    {
        void Run();

        bool IsMigrated();
    }
}
