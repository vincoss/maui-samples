
## Ensure Entity Framework global tools
```
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef
dotnet ef --version
```

## Migrations
Add a migration with, ensure project directory 'Sqlite_EF_Samples_Library' use CMD

```
dotnet ef migrations add InitialCreate
```