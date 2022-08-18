using Logging_Startup_Samples.Interface;
using Logging_Startup_Samples.Logging;
using Logging_Startup_Samples.Services;

namespace Logging_Startup_Samples
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }
        private static InbuiltLogger _logger = null;

        public App()
        {
            InitializeComponent();
            SetupLogging();

            _logger = InbuiltLog.For(typeof(App));
            _logger.Debug($"Constructor - {nameof(App)}");

            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            _logger.Debug($"Constructor - {nameof(App)}");

            MainPage = new AppShell();
        }

        protected async override void OnStart()
        {
            base.OnStart();

            _logger.Debug($"Begin - {nameof(OnStart)}");

            var startup = App.ServiceProvider.GetService<Startup>();
            await startup.RunAsync();

            _logger.Debug($"End - {nameof(OnStart)}");
        }

        private void SetupLogging()
        {
            var path = ServiceProvider.GetService<IPath>();

            var fileLoggerPath = path.GetLogFilePath();
            var fileLogger = new InbuiltFileLogger(fileLoggerPath);
            var consoleLogger = new InbuiltConsoleLogger();
            var multiLogger = new InbuiltMultipleLoggerSink(consoleLogger, fileLogger);

            var factory = new InbuiltMultipleLoggerFactory(multiLogger);
            InbuiltLog.SetFactory(factory);
        }

        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            if (e.Exception == null)
            {
                return;
            }
            var exception = new Exception("AppDomain_CurrentDomain_FirstChanceException", e.Exception);
            LogException(exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex == null)
            {
                return;
            }
            var exception = new Exception("AppDomain_CurrentDomain_UnhandledException", ex);
            LogException(exception);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (e.Exception == null)
            {
                return;
            }
            var exception = new Exception("TaskScheduler_UnobservedTaskException", e.Exception);
            LogException(exception);
        }

        private void LogException(Exception exception)
        {
            _logger.Error(exception, exception.Message);
        }
    }
}