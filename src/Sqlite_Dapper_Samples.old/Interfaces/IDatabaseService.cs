using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite_Dapper_Samples.Interfaces
{
    public interface IDatabaseService
    {
        string DatabaseName { get; }
        string ConnectionString { get; }
    }
}
