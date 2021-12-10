using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilePicker_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnPicker(object sender, EventArgs e)
        {
            var filePath = await PickFileAsync(null);
            lblInfo.Text = filePath;
        }

        public async Task<string> PickFileAsync(string[] allowedTypes = null)
        {
            if (allowedTypes == null)
            {
                allowedTypes = new string[0];
            }

            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, allowedTypes },
                { DevicePlatform.iOS, allowedTypes },
                { DevicePlatform.macOS, allowedTypes },
                { DevicePlatform.tvOS, allowedTypes },
                { DevicePlatform.Tizen, allowedTypes },
                { DevicePlatform.UWP, allowedTypes },
                { DevicePlatform.watchOS, allowedTypes },
                { DevicePlatform.Unknown, allowedTypes },
            });

            var options = new PickOptions
            {
                PickerTitle = "Pick a file",
                FileTypes = customFileType
            };

            var result = await FilePicker.PickAsync(options);

            if (result != null)
            {
                return result.FullPath;
            }
            return null;
        }
    }
}
