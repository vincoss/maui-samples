using Blazor_AppWithWebServer_EmbedIO.Controllers;
using EmbedIO;
using EmbedIO.WebApi;
using Microsoft.Extensions.Logging;
using Swan.Logging;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;


namespace Blazor_AppWithWebServer_EmbedIO.Services
{
    public class ServerOptions
    {
        public int Port { get; set; } = 5167;
    }

    public interface IEmbedServer : IDisposable
    {
        void Start();

        void Stop();

        IEnumerable<string> GetLogs();

        string GetBaseUrl();

        string GetLocalIp();
    }

    public class EmbedIOServerService : IEmbedServer
    {
        private bool _isRunning;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmbedIOServerService> _logger;
        private readonly ServerOptions _serverOptions;
        private readonly LocalServerLogger _serverLogger;
        private readonly IPath _path;
        private CancellationTokenSource? _cts;

        public EmbedIOServerService(IServiceProvider serviceProvider, ILogger<EmbedIOServerService> logger, ServerOptions serverOptions, IPath path)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serverOptions = serverOptions ?? throw new ArgumentNullException(nameof(serverOptions));
            _serverLogger = new LocalServerLogger(_logger);
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        /// <summary>
        /// Logs only used for the UI to view.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetLogs()
        {
            return _serverLogger.Logs;
        }

        /// <summary>
        /// Start server.
        /// </summary>
        public void Start()
        {
            _logger.LogDebug(nameof(Start));

            if (_isRunning)
            {
                return;
            }

            EnsureWwwRootFiles();

            Task.Factory.StartNew(() =>
            {
                RunInternal();
            });
        }

        /// <summary>
        /// Stop server.
        /// </summary>
        public void Stop()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            if (_serverLogger != null)
            {
                _serverLogger.Dispose();
            }
        }

        public string GetBaseUrl()
        {
            var _port = _serverOptions.Port;
            var ip = GetLocalIp();
            var url = $"http://{ip}:{_port}";
            return url;
        }

        public string GetLocalIp()
        {
            string localIP;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }

        private void RunInternal()
        {
            using (_cts = new CancellationTokenSource(Timeout.InfiniteTimeSpan))
            {
                var baseUrl = GetBaseUrl();

                Task.WaitAll(
                    RunWebServerAsync(baseUrl, _cts.Token),
                    WaitForUserBreakAsync(_cts.Token));
            }
        }

        private async Task RunWebServerAsync(string baseUrl, CancellationToken cancellationToken)
        {
            using var server = CreateWebServer(baseUrl);
            await server.RunAsync(cancellationToken).ConfigureAwait(false);
        }

        private WebServer CreateWebServer(string baseUrl)
        {
            _logger.LogDebug(baseUrl);

            Swan.Logging.Logger.RegisterLogger(_serverLogger);

            var options = new WebServerOptions();
            options.AddUrlPrefix(baseUrl);
            options.Mode = HttpListenerMode.EmbedIO;

            var server = new WebServer(options);

            server.WithCors();
            server.WithWebApi("/api", m => m.WithController(() => (TestController)_serviceProvider.GetService(typeof(TestController))));
            server.WithStaticFolder("/", _path.GetWwwRootPath(), true);
            _isRunning = true;

            // Listen for state changes.
            server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();

            return server;
        }

        private static async Task WaitForUserBreakAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            while (cancellationToken.IsCancellationRequested == false) { }
        }

        private string[] GetAllWwwRootEmbeddedFiles()
        {
            var assembly = typeof(EmbedIOServerService).Assembly;
            string folderName = string.Format("{0}.EmbeddedWebsite", assembly.GetName().Name);
            return assembly.GetManifestResourceNames()
                           .Where(r => r.StartsWith(folderName))
                           .ToArray();
        }

        /// <summary>
        /// Copy embedded files to server static folder.
        /// </summary>
        private void EnsureWwwRootFiles()
        {
            var files = GetAllWwwRootEmbeddedFiles();
            var destinationDir = _path.GetWwwRootPath();
            var assembly = typeof(EmbedIOServerService).Assembly;

            foreach (var item in files)
            {
                const string cssKey = "css.";
                const string jsKey = "js.";

                var fileName = item.Replace($"{assembly.GetName().Name}.EmbeddedWebsite.", "");
                var isCss = fileName.StartsWith(cssKey, StringComparison.OrdinalIgnoreCase);
                var isJs = fileName.StartsWith(jsKey, StringComparison.OrdinalIgnoreCase);

                // Fix file name
                fileName = fileName.Replace("css.", "").Replace("js.", "");

                var css = isCss ? "css" : "";
                var js = isJs ? "js" : "";

                var destinationDirectory = Path.Combine(destinationDir, css, js);
                var destinationFileName = Path.Combine(destinationDirectory, fileName);

                if (Directory.Exists(destinationDirectory) == false) Directory.CreateDirectory(destinationDirectory);

                var source = EmbedServerExtensions.ReadResourceFileStream(item, assembly);

                using (var fileStream = File.Create(destinationFileName))
                {
                    source.BaseStream.CopyTo(fileStream);
                }
            }
        }

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Stop();
        }

        #endregion

        class LocalServerLogger : Swan.Logging.ILogger
        {
            private readonly ILogger<EmbedIOServerService> _logger;
            private readonly IList<string> _logs = new List<string>();

            public LocalServerLogger(ILogger<EmbedIOServerService> logger)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public Swan.Logging.LogLevel LogLevel => Swan.Logging.LogLevel.Trace;

            public IEnumerable<string> Logs => _logs.Where(x => x != null).Select(x => x.ToString());

            public void Log(Swan.Logging.LogMessageReceivedEventArgs logEvent)
            {
                _logger.LogDebug(logEvent.Message);

                string log = $"{GetPretext(logEvent)} {logEvent.Message}";

                // Collect server logs to display on UI.

                if (string.IsNullOrWhiteSpace(log) == false)
                {
                    _logs.Insert(0, log);
                }

                // Self clean
                const int MaxLogs = 1000;

                if (_logs.Count > MaxLogs)
                {
                    var items = _logs.Skip(MaxLogs).Select((item, index) => (item, index));
                    foreach (var item in items)
                    {
                        _logs.RemoveAt(item.index);
                    }
                }
            }

            public static string GetPretext(Swan.Logging.LogMessageReceivedEventArgs logEvent)
            {
                string pretext;
                const string DateTimeFormat = "yyyy-MM-dd-HH:mm:ss.fffffff zzz";
                var loggerType = logEvent.CallerMemberName;

                switch (logEvent.MessageType)
                {
                    case Swan.Logging.LogLevel.Info:
                        pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}] [INF]";
                        break;
                    case Swan.Logging.LogLevel.Debug:
                        pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}] [DBG]";
                        break;
                    case Swan.Logging.LogLevel.Warning:
                        pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}] [WRN]";
                        break;
                    case Swan.Logging.LogLevel.Error:
                        pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}] [ERR]";
                        break;
                    case Swan.Logging.LogLevel.Fatal:
                        pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}][FTL]";
                        break;
                    case Swan.Logging.LogLevel.Trace:
                        pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}][TRC]";
                        break;
                    case Swan.Logging.LogLevel.None:
                        pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}][NON]";
                        break;
                    default:
                        pretext = "";
                        break;
                }
                return pretext;
            }

            #region IDisposable

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (_logs != null)
                {
                    _logs.Clear();
                }
            }

            #endregion
        }

    }
}