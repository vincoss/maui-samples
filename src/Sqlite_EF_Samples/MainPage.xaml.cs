using Sqlite_EF_Samples_Library.Entities;
using Sqlite_EF_Samples_Library.Interfaces;
using Microsoft.EntityFrameworkCore;


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

            var databaseService = App.ServiceProvider.GetService<IDatabaseService>();
            using var db = new DatabaseContext(databaseService.ConnectionString);
            {
                var count = await db.Items.CountAsync();

                txtMessage.Text = $"Total items: {count}";
            }
        }
    }
}