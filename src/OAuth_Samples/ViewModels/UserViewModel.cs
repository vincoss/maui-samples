using OAuth_Samples.Pages;
using ShortMvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace OAuth_Samples.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public UserViewModel()
        {
            LoginCommand = new Command(OnLoginCommand);
        }

        public override void Initialize()
        {
            UserName = "User Name";
            ShowUser = false;
            ShowLogin = !ShowUser;
        }

        private async void OnLoginCommand()
        {
            var page = App.ServiceProvider.GetService<LoginPage>();
            await App.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private bool _showUser;

        public bool ShowUser
        {
            get { return _showUser; }
            set { SetProperty(ref _showUser, value); }
        }

        private bool _showLogin;

        public bool ShowLogin
        {
            get { return _showLogin; }
            set { SetProperty(ref _showLogin, value); }
        }
    }
}
