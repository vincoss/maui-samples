using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace Logging_Startup_Samples.Logging
{
    public enum InbuiltLogLevel : byte
    {
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }

    public interface InbuiltLogger
    {
        bool IsEnabled(InbuiltLogLevel level);

        void Log(InbuiltLogLevel level, Exception exception, string format, params object[] args);
    }

    class InbuiltLoggerImpl : InbuiltLogger
    {
        private readonly InbuiltLoggerSink _sink;
        private readonly Type _type;

        public InbuiltLoggerImpl(InbuiltLoggerSink sink, Type type)
        {
            _sink = sink ?? throw new ArgumentNullException(nameof(sink));
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public bool IsEnabled(InbuiltLogLevel level)
        {
            return _sink.IsEnabled(level);
        }

        public void Log(InbuiltLogLevel level, Exception exception, string format, params object[] args)
        {
            _sink.Log(_type, level, exception, format, args);
        }
    }

    public interface InbuiltLoggerSink
    {
        bool IsEnabled(InbuiltLogLevel level);

        void Log(Type type, InbuiltLogLevel level, Exception exception, string format, params object[] args);
    }

    public interface InbuiltLoggerFactory
    {
        InbuiltLogger CreateLogger(Type type);
    }

    public sealed class InbuiltLog
    {
        private static InbuiltLoggerFactory _factory;

        static InbuiltLog()
        {
            _factory = new InbuiltNullLoggerFactory();
        }

        #region For
        public static InbuiltLogger For(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return GetLogger(type);
        }

        public static InbuiltLogger For(object itemThatRequiresLoggingServices)
        {
            if (itemThatRequiresLoggingServices == null)
            {
                throw new ArgumentNullException(nameof(itemThatRequiresLoggingServices));
            }
            return For(itemThatRequiresLoggingServices.GetType());
        }

        public static void SetFactory(InbuiltLoggerFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            _factory = factory;
        }

        #endregion

        #region GetLogger

        private static InbuiltLogger GetLogger(Type type)
        {
            return _factory.CreateLogger(type);
        }

        #endregion
    }

    public static class InbuiltLoggingExtensions
    {
        private const string DateTimeFormat = "yyyy-MM-dd-HH:mm:ss.fffffff zzz";

        public static void Debug(this InbuiltLogger logger, string message)
        {
            LogInternal(logger, InbuiltLogLevel.Debug, null, message, null);
        }

        public static void Debug(this InbuiltLogger logger, Exception exception, string message)
        {
            LogInternal(logger, InbuiltLogLevel.Debug, exception, message, null);
        }

        public static void Debug(this InbuiltLogger logger, string format, params object[] args)
        {
            LogInternal(logger, InbuiltLogLevel.Debug, null, format, args);
        }

        public static void Debug(this InbuiltLogger logger, Exception exception, string format, params object[] args)
        {
            LogInternal(logger, InbuiltLogLevel.Debug, exception, format, args);
        }

        public static void Error(this InbuiltLogger logger, string message)
        {
            LogInternal(logger, InbuiltLogLevel.Error, null, message, null);
        }

        public static void Error(this InbuiltLogger logger, Exception exception, string message)
        {
            LogInternal(logger, InbuiltLogLevel.Error, exception, message, null);
        }

        public static void Error(this InbuiltLogger logger, string format, params object[] args)
        {
            LogInternal(logger, InbuiltLogLevel.Error, null, format, args);
        }

        public static void Error(this InbuiltLogger logger, Exception exception, string format, params object[] args)
        {
            LogInternal(logger, InbuiltLogLevel.Error, exception, format, args);
        }

        public static void Warning(this InbuiltLogger logger, string message)
        {
            LogInternal(logger, InbuiltLogLevel.Warning, null, message, null);
        }

        public static void Info(this InbuiltLogger logger, string message)
        {
            LogInternal(logger, InbuiltLogLevel.Information, null, message, null);
        }

        public static string GetPretext(InbuiltLogLevel level, Type loggerType)
        {
            string pretext;
            switch (level)
            {
                case InbuiltLogLevel.Information:
                    pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}] [INF]";
                    break;
                case InbuiltLogLevel.Debug:
                    pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}] [DBG]";
                    break;
                case InbuiltLogLevel.Warning:
                    pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}] [WRN]";
                    break;
                case InbuiltLogLevel.Error:
                    pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}] [ERR]";
                    break;
                case InbuiltLogLevel.Fatal:
                    pretext = $"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{loggerType}] [{Thread.CurrentThread.ManagedThreadId}][FTL]";
                    break;
                default:
                    pretext = "";
                    break;
            }
            return pretext;
        }

        #region Private methods

        private static void LogInternal(InbuiltLogger logger, InbuiltLogLevel level, Exception exception, string format, object[] objects)
        {
            if (logger.IsEnabled(level))
            {
                logger.Log(level, exception, format, objects);
            }
        }

        #endregion
    }

    public class InbuiltConsoleLogger : InbuiltLoggerSink
    {
        public bool IsEnabled(InbuiltLogLevel level)
        {
            return true;
        }

        public void Log(Type type, InbuiltLogLevel level, Exception exception, string format, params object[] args)
        {
            string message = format;
            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            string log = $"{InbuiltLoggingExtensions.GetPretext(level, type)} {message}\r\n";

            Console.WriteLine(log);

            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }
    }

    public class InbuiltConsoleLoggerFactory : InbuiltLoggerFactory
    {
        private readonly InbuiltLoggerSink _sink = new InbuiltConsoleLogger();

        public InbuiltLogger CreateLogger(Type type)
        {
            return new InbuiltLoggerImpl(_sink, type);
        }
    }

    public class InbuiltFileLoggerFactory : InbuiltLoggerFactory
    {
        private readonly InbuiltLoggerSink _sink;

        public InbuiltFileLoggerFactory(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));

            _sink = new InbuiltFileLogger(filePath);
        }

        public InbuiltLogger CreateLogger(Type type)
        {
            return new InbuiltLoggerImpl(_sink, type);
        }
    }

    public class InbuiltNullLogger : InbuiltLoggerSink
    {
        public bool IsEnabled(InbuiltLogLevel level)
        {
            return false;
        }

        public void Log(Type type, InbuiltLogLevel level, Exception exception, string format, params object[] args)
        {
            // Hola
        }
    }

    public class InbuiltNullLoggerFactory : InbuiltLoggerFactory
    {
        private readonly InbuiltLoggerSink _sink = new InbuiltNullLogger();

        public InbuiltLogger CreateLogger(Type type)
        {
            return new InbuiltLoggerImpl(_sink, type);
        }
    }

    public class InbuiltFileLogger : InbuiltLoggerSink
    {
        private readonly string _logPath;

        public InbuiltFileLogger(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

            _logPath = path;
        }

        public bool IsEnabled(InbuiltLogLevel level)
        {
            return true;
        }

        public void Log(Type type, InbuiltLogLevel level, Exception exception, string format, params object[] args)
        {
            string message = format;
            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            string log = $"{InbuiltLoggingExtensions.GetPretext(level, type)} {message}\r\n";

            Write(log);

            if (exception != null)
            {
                Write($"{exception}\r\n");
            }
        }

        private void Write(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return;
            }
            using (var fs = CreateStream(_logPath, true, FileShare.ReadWrite))
            {
                byte[] info = Encoding.UTF8.GetBytes(str);
                fs.Write(info, 0, info.Length);
            }
        }

        private Stream CreateStream(string filename, bool append, FileShare fileShare)
        {
            string directoryFullName = Path.GetDirectoryName(filename);

            if (!Directory.Exists(directoryFullName))
            {
                Directory.CreateDirectory(directoryFullName);
            }

            FileMode fileOpenMode = append ? FileMode.Append : FileMode.Create;
            return new FileStream(filename, fileOpenMode, FileAccess.Write, fileShare);
        }
    }

    public class InbuiltMultipleLoggerSink : InbuiltLoggerSink
    {
        private readonly InbuiltLoggerSink[] _sinks;

        public InbuiltMultipleLoggerSink(params InbuiltLoggerSink[] sinks)
        {
            var otherMultipleLogs = sinks.OfType<InbuiltMultipleLoggerSink>().ToArray();

            _sinks = sinks
                .Except(otherMultipleLogs)
                .Concat(otherMultipleLogs.SelectMany(l => l._sinks))
                .ToArray();
        }

        public bool IsEnabled(InbuiltLogLevel level)
        {
            return true;
        }

        public void Log(Type type, InbuiltLogLevel level, Exception exception, string format, params object[] args)
        {
            foreach (var log in _sinks)
            {
                log.Log(type, level, exception, format, args);
            }
        }
    }

    public class InbuiltMultipleLoggerFactory : InbuiltLoggerFactory
    {
        private readonly InbuiltLoggerSink _sink;

        public InbuiltMultipleLoggerFactory(InbuiltMultipleLoggerSink sink)
        {
            _sink = sink ?? throw new ArgumentNullException(nameof(sink));
        }

        public InbuiltLogger CreateLogger(Type type)
        {
            return new InbuiltLoggerImpl(_sink, type);
        }
    }
}