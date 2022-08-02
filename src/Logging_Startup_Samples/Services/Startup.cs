using Logging_Startup_Samples.Interface;


namespace Logging_Startup_Samples.Services
{
    public class Startup
    {
        private readonly IPath _path;

        public Startup(IPath path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public Task RunAsync()
        {
            EnsureDirectories();
            ClearOldLogFiles();
            return Task.CompletedTask;
        }

        private void EnsureDirectories()
        {
            if (Directory.Exists(_path.GetLogsFolder()) == false) Directory.CreateDirectory(_path.GetLogsFolder());
        }

        private void ClearOldLogFiles()
        {
            var files = Directory.GetFiles(_path.GetLogsFolder());

            foreach (string file in files)
            {
                var fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddDays(-14))
                {
                    fi.Delete();
                }
            }
        }
    }
}
