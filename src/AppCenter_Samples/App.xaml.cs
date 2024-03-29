﻿using AppCenter_ConfigurationSample.Services;
using Application = Microsoft.Maui.Controls.Application;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
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
                "uwp=;" +
                "android=;",
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
