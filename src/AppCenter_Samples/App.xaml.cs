using AppCenter_ConfigurationSample.Services;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter;
using System.Text;

namespace AppCenter_Samples
{
    public partial class App : Application
    {
        private readonly IUpdateService _updateService = new UpdateService();

        public App()
        {
            InitializeComponent();

            var page = new MainPage();
            page.UpdateService = _updateService;

            MainPage = page;
        }

        protected override void OnStart()
        {
            // Check if new release is available : https://docs.microsoft.com/en-us/appcenter/sdk/distribute/xamarin
            Distribute.ReleaseAvailable = OnReleaseAvailable;

            Microsoft.AppCenter.AppCenter.Start(
                "uwp=90d88e86-8d19-4378-93a0-bad9631a3e27;" +
                "android=151d3d0f-5598-4f52-9ddc-9bdc2ff361b8;",
                            typeof(Analytics),
                            typeof(Crashes),
                            typeof(Distribute));

            Analytics.TrackEvent("App Started");
        }

        private bool OnReleaseAvailable(ReleaseDetails releaseDetails)
        {
            Analytics.TrackEvent("Release Available.");

            var info = GetUpdateDetail(releaseDetails);

            Analytics.TrackEvent(info);

            _updateService.Update = info;

            return true;
        }

        public static string GetUpdateDetail(ReleaseDetails info)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{nameof(info.Id)}: {info.Id}");
            sb.AppendLine($"{nameof(info.Version)}: {info.Version}");
            sb.AppendLine($"{nameof(info.ShortVersion)}: {info.ShortVersion}");
            sb.AppendLine($"{nameof(info.ReleaseNotes)}: {info.ReleaseNotes}");
            sb.AppendLine($"{nameof(info.ReleaseNotesUrl)}: {info.ReleaseNotesUrl}");
            sb.AppendLine($"{nameof(info.MandatoryUpdate)}: {info.MandatoryUpdate}");

            return sb.ToString();
        }
    }
}
