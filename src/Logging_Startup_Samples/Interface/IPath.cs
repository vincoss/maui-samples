using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging_Startup_Samples.Interface
{
    public interface IPath
    {
        string AppRoot { get; }

        string GetLogsFolder();

        string GetLogFilePath();
    }
}
