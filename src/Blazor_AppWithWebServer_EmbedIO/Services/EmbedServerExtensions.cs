using System.Reflection;


namespace Blazor_AppWithWebServer_EmbedIO.Services
{
    public static class EmbedServerExtensions
    {
        public static StreamReader ReadResourceFileStream(string fileName, Assembly assembly)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            var stream = assembly.GetManifestResourceStream(fileName);
            if (stream == null || stream.Length <= 0)
            {
                throw new FileNotFoundException($"Could not find embedded resource: {fileName}");
            }

            return new StreamReader(stream);
        }
    }
}
