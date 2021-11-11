using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using ViewModelIocSample.Views;
using Application = Microsoft.Maui.Controls.Application;

namespace ViewModelIocSample
{
	public partial class App : Application
	{
		public App(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

			InitializeComponent();
		}

		protected override Window CreateWindow(IActivationState activationState)
		{
			return new Window(new NavigationPage(new HomeView())) { Title = "ViewModelIocSample" };
		}

		public static IServiceProvider ServiceProvider { get; private set; }
	}
}
