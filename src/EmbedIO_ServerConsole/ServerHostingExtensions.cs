using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO_ServerConsole.Controllers;
using System.Reflection;


namespace EmbedIO_ServerConsole
{
    public static class ServerHostingExtensions
    {
        private static int _port = 5167;
        private static string _ip = "http://localhost:";
        public static string Url = $"{_ip}{_port}";

        public static async Task Run(string[] args)
        {
            using (var ctSource = new CancellationTokenSource())
            {
                var serverTask = await Task.Factory.StartNew(async () =>
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

                        await server.RunAsync(ctSource.Token).ConfigureAwait(false);
                    }
                });

                Task.WaitAll(serverTask);
            }
        }
    }
}
