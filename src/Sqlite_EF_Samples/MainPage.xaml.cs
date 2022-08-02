using Sqlite_EF_Samples_Library.Entities;
using Sqlite_EF_Samples_Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Sqlite_EF_Samples
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

            var connection = App.ServiceProvider.GetService<DbConnection>();
            using var db = new DatabaseContext(connection);
            {
                var count = await db.Items.CountAsync();

                txtMessage.Text = $"Total items: {count}";
            }
        }
    }
}