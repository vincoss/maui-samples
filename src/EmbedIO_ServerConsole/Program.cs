using System.Threading.Tasks;
using System;
using System.Reflection;
using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO_ServerConsole.Controllers;


namespace EmbedIO_ServerConsole
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            await ServerStarter();
            Console.Read();
            return 0;
        }

        private static int _port = 5167;
        private static string _ip = "http://localhost:";
        public static string Url = $"{_ip}{_port}";

        public static async Task ServerStarter()
        {
            await Task.Factory.StartNew(async () =>
            {
                using (var server = new WebServer(HttpListenerMode.EmbedIO, Url))
                {
                    Assembly assembly = typeof(Program).Assembly;
                    server.WithWebApi("/api", m => m.WithController(() => new TestController()));
                    await server.RunAsync();
                }
            });
        }
    }
}
