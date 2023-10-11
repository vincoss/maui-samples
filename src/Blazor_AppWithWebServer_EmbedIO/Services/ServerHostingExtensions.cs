using Blazor_AppWithWebServer_EmbedIO.Controllers;
using EmbedIO;
using EmbedIO.WebApi;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
            var ip = GetLocalIp();

            int _port = 5167;
            string _ip = "http://localhost:";

            var platform = DeviceInfo.Platform;

            //if (platform == DevicePlatform.Android)
            //{
                _ip = _ip.Replace("localhost", ip.ToString());
            //}

            string Url = $"{_ip}{_port}";
            return Url;
        }

        private static string GetLocalIp()
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