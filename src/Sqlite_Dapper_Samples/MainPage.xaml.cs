using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Sqlite_Dapper_Samples.Interfaces;
using System;
using System.Linq;

namespace Sqlite_Dapper_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            txtMessage.Text = "Loading...";

            var databaseService = App.ServiceProvider.GetService<IDatabaseService>();

            var sql = @"SELECT COUNT(*) FROM Item";

            using (var connection = new SqliteConnection(databaseService.ConnectionString))
            using (var multi = await connection.QueryMultipleAsync(sql))
            {
                var count = multi.Read<int>().Single();
                txtMessage.Text = $"Total items: {count}";
            }
        }

    }
}
