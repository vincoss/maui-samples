using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Sqlite_Dapper_Samples.Interfaces;
using System;
using Application = Microsoft.Maui.Controls.Application;

namespace Sqlite_Dapper_Samples
{
    public partial class App : Application
    {
		private readonly IBootstrap _bootstrap;

		public App(IServiceProvider serviceProvider, IBootstrap bootstrap)
		{
			ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			_bootstrap = bootstrap ?? throw new ArgumentNullException(nameof(_bootstrap));

			InitializeComponent();

			_bootstrap.Run();
		}

		protected override Window CreateWindow(IActivationState activationState)
		{
			return new Window(new NavigationPage(new MainPage())) { Title = "Sqlite_Dapper_Samples" };
		}

		public static IServiceProvider ServiceProvider { get; private set; }
	}
}
