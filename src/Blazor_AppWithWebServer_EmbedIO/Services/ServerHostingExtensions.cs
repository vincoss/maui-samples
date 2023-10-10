using Blazor_AppWithWebServer_EmbedIO.Controllers;
using EmbedIO;
using EmbedIO.WebApi;
using System.Reflection;
using System.Threading;


namespace EmbedIO_ServerConsole
{
    public static class ServerHostingExtensions
    {
        private static int _port = 5167;
        private static string _ip = "http://localhost:";
        public static string Url = $"{_ip}{_port}";

        public static void Run(string[] args)
        {
            Task.Factory.StartNew(async () =>
            {
                using (var server = new WebServer(HttpListenerMode.EmbedIO, Url))
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
    }
}
