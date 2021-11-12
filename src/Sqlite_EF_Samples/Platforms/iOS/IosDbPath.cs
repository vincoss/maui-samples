using Sqlite_EF_Samples_Library.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sqlite_EF_Samples.Platforms.iOS
{
    public class DbPath : IPath
    {
        public string GetDatabasePath(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
