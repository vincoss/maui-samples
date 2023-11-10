using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_AppWithWebServer_EmbedIO.Services
{
    public interface IPath
    {
        string AppRoot { get; }

        string GetWwwRootPath();

    }

    public class PathService : IPath
    {
        private const string WwwRootFolder = "wwwroot";

        public string AppRoot => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public string GetWwwRootPath()
        {
            return Path.Combine(AppRoot, WwwRootFolder);
        }
    }
}
