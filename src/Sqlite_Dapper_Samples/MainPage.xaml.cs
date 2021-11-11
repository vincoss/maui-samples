using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;

namespace Sqlite_Dapper_Samples
{
    public partial class MainPage : ContentPage
    {
		public MainPage()
		{
			InitializeComponent();
		}

		int count = 0;
		private void OnCounterClicked(object sender, EventArgs e)
		{
			count++;
			CounterLabel.Text = $"Current count: {count}";
		}
	}
}
