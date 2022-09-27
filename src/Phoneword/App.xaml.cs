using Phoneword.Views;

namespace Phoneword
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new OneView();
        }
    }
}