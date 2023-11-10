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
        string GetGalleryFolder();

        string GetWwwRootPath();

    }

    public class PathService : IPath
    {
        private const string GalleryFolder = "Gallery";
        private const string WwwRootFolder = "wwwroot";

        public string AppRoot => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public string GetGalleryFolder()
        {
            return Path.Combine(AppRoot, GalleryFolder);
        }

        public string GetWwwRootPath()
        {
            return Path.Combine(AppRoot, WwwRootFolder);
        }
    }
}
