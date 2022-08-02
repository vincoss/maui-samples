using Logging_Startup_Samples.Interface;


namespace Logging_Startup_Samples.Services
{
    public class PathService : IPath
    {
        public string AppRoot => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public string GetLogsFolder()
        {
            return Path.Combine(AppRoot, "Logs");
        }

        public string GetLogFilePath()
        {
            return Path.Combine(GetLogsFolder(), $"{DateTime.UtcNow:yyyyMMddHHmmssfffff}-logging-samples-log.txt");
        }
    }
}
