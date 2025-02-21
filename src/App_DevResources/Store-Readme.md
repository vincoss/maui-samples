
# Android

https://play.google.com/console/u/0/developers/

## Publish the app command line
`
dotnet publish -c Release -f:net8.0-android
dotnet publish -c Release -f:net9.0-android
`
Upload the .aap package to the store

# Windows
https://partner.microsoft.com/en-us/dashboard/apps-and-games/overview

## Publish app
https://learn.microsoft.com/en-us/dotnet/maui/windows/deployment/publish-cli?view=net-maui-8.0
NOTE: Ensure that CMD path is in app folder not in the solution folder

`
dotnet publish -f net8.0-windows10.0.19041.0 -c Release --self-contained
dotnet publish -f net9.0-windows10.0.19041.0 -c Release --self-contained
`
# iOS
https://appstoreconnect.apple.com/apps/

## Publish

1. Build app and deploy to real device
2. Publish The App (This will archive the app)
3. Log into the Mac Mini and go to Archives and find the app
4. Push to store
5. Log to the store and create new release
		
		