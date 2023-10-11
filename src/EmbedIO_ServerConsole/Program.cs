using System.Threading.Tasks;
using System;


namespace EmbedIO_ServerConsole
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            await ServerHostingExtensions.Run(args);
            Console.Read();
            return 0;
        }
    }
}