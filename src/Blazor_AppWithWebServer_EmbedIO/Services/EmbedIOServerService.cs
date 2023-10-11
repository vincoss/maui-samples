using Blazor_AppWithWebServer_EmbedIO.Controllers;
using EmbedIO;
using EmbedIO.WebApi;
using Swan.Logging;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;


namespace Blazor_AppWithWebServer_EmbedIO.Services
{
    public interface IEmbedServer
    {
        void Run(string[] args);

        IEnumerable<string> GetLogs();

        string GetBaseUrl();

        string GetLocalIp();
    }

    public class EmbedIOServerService : IEmbedServer
    {
        private bool _isRunning;
        private IList<string> _logs = new List<string>();

        public IEnumerable<string> GetLogs()
        {
            return _logs;
        }

        public void Run(string[] args)
        {
            if(_isRunning)
            {
                return;
            }

            Task.Factory.StartNew(async () =>
            {
                var url = GetBaseUrl();

                using (var server = new WebServer(HttpListenerMode.EmbedIO, url))
                {
                    server.WithWebApi("/api", m => m.WithController(() => new TestController()));

                    // Listen for state changes.
                    server.StateChanged += (s, e) =>
                    {
                        _logs.Insert(0, e.NewState.ToString());

                        // TODO: delete old logs about 1000
                    };

                    _isRunning = true;
                    await server.RunAsync().ConfigureAwait(false);
                }
            });
        }

        public string GetBaseUrl()
        {
            var _port = 5167; // TODO: port configurable
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
    }
}