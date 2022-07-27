using OidcSamples.Services;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Diagnostics;

namespace OidcSamples
{
    public partial class MainPage : ContentPage
    {
        public const string CallbackUri = "myapp";
        public static readonly string CallbackScheme = $"{CallbackUri}:/authenticated";
        public static readonly string SignoutCallbackScheme = $"{CallbackUri}:/signout-callback-oidc";

        public MainPage()
        {
            InitializeComponent();

            var model = new MainViewModel();
            this.BindingContext = model;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string AuthorityUrl = "";
        private Credentials? _credentials;
        private readonly OidcIdentityService _oidcIdentityService;

        public MainViewModel()
        {
            // const string scope = "openid profile offline_access";
            const string scope = "";
            _oidcIdentityService = new OidcIdentityService("", MainPage.CallbackScheme, MainPage.SignoutCallbackScheme, scope, AuthorityUrl);
            ExecuteLogin = new Command(Login);
            ExecuteRefresh = new Command(RefreshTokens);
            ExecuteLogout = new Command(Logout);
            ExecuteGetInfo = new Command(GetInfo);
            ExecuteCopyAccessToken = new Command(async () => await Clipboard.SetTextAsync(_credentials?.AccessToken));
            ExecuteCopyIdentityToken = new Command(async () => await Clipboard.SetTextAsync(_credentials?.IdentityToken));
        }

        public ICommand ExecuteLogin { get; }
        public ICommand ExecuteRefresh { get; }
        public ICommand ExecuteLogout { get; }
        public ICommand ExecuteGetInfo { get; }
        public ICommand ExecuteCopyAccessToken { get; }
        public ICommand ExecuteCopyIdentityToken { get; }

        public string TokenExpirationText => "Access Token expires at: " + _credentials?.AccessTokenExpiration;
        public string AccessTokenText => "Access Token: " + _credentials?.AccessToken;
        public string IdTokenText => "Id Token: " + _credentials?.IdentityToken;
        public bool IsLoggedIn => _credentials != null;
        public bool IsNotLoggedIn => _credentials == null;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void GetInfo()
        {
            var url = Path.Combine(AuthorityUrl, "manage", "index");
            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (string.IsNullOrEmpty(_credentials?.RefreshToken))
                {
                    // no valid refresh token exists => authenticate
                    await _oidcIdentityService.Authenticate();
                }
                else
                {
                    // we have a valid refresh token => refresh tokens
                    await _oidcIdentityService.RefreshToken(_credentials!.RefreshToken);
                }
            }

            var str = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(str);
        }

        private async void Login()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                // Connection to internet is available
            }

            Credentials credentials = await _oidcIdentityService.Authenticate();
            UpdateCredentials(credentials);

            _httpClient.DefaultRequestHeaders.Authorization = credentials.IsError
                ? null
                : new AuthenticationHeaderValue("bearer", credentials.AccessToken);
        }

        private async void RefreshTokens()
        {
            if (_credentials?.RefreshToken == null) return;
            Credentials credentials = await _oidcIdentityService.RefreshToken(_credentials.RefreshToken);
            UpdateCredentials(credentials);
        }

        private async void Logout()
        {
            await _oidcIdentityService.Logout(_credentials?.IdentityToken);
            _credentials = null;
            OnPropertyChanged(nameof(TokenExpirationText));
            OnPropertyChanged(nameof(AccessTokenText));
            OnPropertyChanged(nameof(IdTokenText));
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsNotLoggedIn));
        }

        private void UpdateCredentials(Credentials credentials)
        {
            _credentials = credentials;
            OnPropertyChanged(nameof(TokenExpirationText));
            OnPropertyChanged(nameof(AccessTokenText));
            OnPropertyChanged(nameof(IdTokenText));
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsNotLoggedIn));
        }
    }
}