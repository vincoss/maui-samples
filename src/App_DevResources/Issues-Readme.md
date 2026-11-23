
## Installation

https://docs.microsoft.com/en-us/dotnet/maui/get-started/installation

### Android paths
```
C:\Program Files (x86)\Android\android-sdk\tools
C:\Users\{UserName}\AppData\Local\Android\Sdk\tools\bin
```

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

## Issue 7 REQUEST_INSTALL_PACKAGES
Publish to Android store
App Status: Rejected
Please remove the use of REQUEST_INSTALL_PACKAGES permission from your app.
 remove <PackageReference Include="Microsoft.AppCenter.Distribute" Version="4.5.3" />
 https://stackoverflow.com/questions/74180335/xamarin-permission-request-install-packages-which-dependency-brings-this-p

 ## iOS
 One or more errors occurred while starting the following Agents: IDB 17.11.0.86

1. Remove the /Users/USERNAME/Library/Caches/Xamarin/XMA/SDKs/dotnet folder on the MacOS
2. .vs remove folder