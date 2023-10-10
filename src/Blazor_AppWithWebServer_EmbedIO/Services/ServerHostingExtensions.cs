using Blazor_AppWithWebServer_EmbedIO.Controllers;
using EmbedIO;
using EmbedIO.WebApi;
using System;
using System.Reflection;
using System.Threading;

namespace EmbedIO_ServerConsole
{
    public static class ServerHostingExtensions
    {
        public static void Run(string[] args)
        {
            Task.Factory.StartNew(async () =>
            {
                var url = GetUrl();

                using (var server = new WebServer(HttpListenerMode.EmbedIO, url))
                {
                    Assembly assembly = typeof(ServerHostingExtensions).Assembly;
                    server.WithWebApi("/api", m => m.WithController(() => new TestController()));

                    // Listen for state changes.
                    server.StateChanged += (s, e) =>
                    {
                        Console.WriteLine(e.NewState);
                    };

                    await server.RunAsync().ConfigureAwait(false);
                }
            });
        }


        public static string GetUrl()
        {
            int _port = 5167;
            string _ip = "http://localhost:";

            var platform = DeviceInfo.Platform;

            if (platform == DevicePlatform.Android)
            {
                _ip = _ip.Replace("localhost", "10.0.2.2");
            }

            string Url = $"{_ip}{_port}";
            return Url;
        }
    }
}