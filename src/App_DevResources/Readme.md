
## Current

CarouselView_Samples

## Tasks
search for todo

```
entry and color picker
	name and color
	color incremental change
multiple entry (on next add new entry with ...)
	empty must remove the entry
```

## Installation

https://docs.microsoft.com/en-us/dotnet/maui/get-started/installation

## Issues

1.	If the maui-check throws lots of errors then go to 'C:\Program Files\dotnet' and remove old preview nuget feeds and packages.
2.	Can't deploy into empulator or the device. Right click on solution then click Configuration Manager and ensure that deploy is checked.

## Issue 3
```
System.TypeLoadException: 'Could not resolve type with token 01000008 from typeref (expected class 'System.Drawing.BitmapSuffixInSatelliteAssemblyAttribute' in assembly 
'System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a')'
```

Solution is to disable Hot Reload

## Issue 4
```
Fix failed - Workload Update failed: `dotnet workload update --no-cache --disable-parallel --from-rollback-file "C:\Users\Ferdinand\AppData\Local\Temp\maui-check-4d185389\workload.json" --source "https://api.nuget.org/v3/index.json"--source "https://pkgs.dev.azure.com/dnceng/public/_packaging/6.0.100-rc.1.21458.32-shipping/nuget/v3/index.json"`
```
Delelete all workloads from
C:\Program Files\dotnet\metadata

run
dotnet workload install maui
dotnet tool update -g Redth.Net.Maui.Check
maui-check

## Issue 5
Cannot deploy android device or emulator

a. Solution see Configuration Manager for the solution ensure that deploy is checked
b. Double click on project and reorder 'TargetFrameworks' targets, make the android first.
c. https://github.com/dotnet/maui/issues/2493

## Issue 6
Cannot create the archive file because the copy of mdbs files failed.
Original archive folder: C:\Users\user-name\AppData\Local\Xamarin\Mono for Android\Archives

a.	This erros might with path lenght, but the error message is not right.
b.	Go to Tools->Options->Xamarin->Android Setting
c.	try to change the Archives path


## Temp

toast sample
app center sample

  public interface IToastMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }

 public class NotificationService : IToastMessage
    {
        public void ShortAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            throw new NotImplementedException();
        }

        public void LongAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            throw new NotImplementedException();
        }
    }

    // Android
public class NotificationService : IToastMessage
    {
        public void ShortAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }

        public void LongAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }
    }

    # windows
     public class NotificationService : IToastMessage
    {
        public void ShortAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            var toast = Get(message);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public void LongAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            var toast = Get(message, DateTimeOffset.Now.AddSeconds(10));
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private ToastNotification Get(string message, DateTimeOffset? expire = null)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText01;

            var toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            var toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(message));

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = expire;
            return toast;
        }
    }

## File picker

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
                PickerTitle = AppResources.PickFile,
                FileTypes = customFileType
            };

            var result = await FilePicker.PickAsync(options);

            if (result != null)
            {
                return result.FullPath;
            }
            return null;
        }
