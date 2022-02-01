
## 

 CollectionView grouping sample groups (Popular, Group A, Group B)
 collection view
collection view grouping
https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/collectionview/grouping

Home
Boards
Folders
My Cards
Settings
Help
About

## Icons
Home
Folder
Board
My Card
Settings
Help
About

Search
Done, check
Times
Ellipsis Horizontal
Ellipsis Vertical
+ Add
Edit
Delete
Template Board
Angle Left
Close
Clone
Share

    <Grid
            ColumnDefinitions="*"
            RowDefinitions="{OnIdiom Phone='100,*', Default='100,*,0'}"
            >
 <VerticalStackLayout Spacing="8">


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
