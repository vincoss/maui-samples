using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_AppWithWebServer_EmbedIO.Services
{
    public interface IStartup
    {
        Task RunAsync();
    }

    public class StartupService : IStartup
    {
        private readonly IPath _path;
        private readonly IEmbedServer _embedServer;

        public StartupService(IPath path, IEmbedServer embedServer)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            _embedServer = embedServer ?? throw new ArgumentNullException(nameof(embedServer));
        }

        public Task RunAsync()
        {
            EnsureDirectories();

            _embedServer.Start();

            return Task.CompletedTask;
        }

        private void EnsureDirectories()
        {
            if (Directory.Exists(_path.GetGalleryFolder()) == false) Directory.CreateDirectory(_path.GetGalleryFolder());
            if (Directory.Exists(_path.GetWwwRootPath()) == false) Directory.CreateDirectory(_path.GetWwwRootPath());
        }
    }
}
