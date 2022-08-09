using OAuth_Samples.Security;
using ShortMvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OAuth_Samples.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly AuthClient _authClient;

        public LoginPageViewModel(AuthClient authClient)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
        }

        public async override Task InitializeAsync()
        {
            var loginResult = await _authClient.LoginAsync();
            var result = loginResult.ToCredentials();

            if (loginResult.IsError == false && result.IsError == false)
            {
                await App.Current.MainPage.Navigation.PopToRootAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", loginResult.ErrorDescription, "OK");
            }
        }
    }
}
