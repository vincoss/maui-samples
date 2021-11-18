using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using System;
using ViewModelIoc_Samples.Views;
using Application = Microsoft.Maui.Controls.Application;

namespace ViewModelIoc_Samples
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
