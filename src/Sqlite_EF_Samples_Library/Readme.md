
## Ensure Entity Framework global tools
```
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef
dotnet tool install --local dotnet-ef
dotnet ef --version
```

## Migrations
Add a migration with, ensure project directory 'Sqlite_EF_Samples_Library' use CMD

```
dotnet ef migrations add InitialCreate
```


## Resources
https://docs.microsoft.com/en-us/ef/core/cli/dotnet
https://www.sqlite.org/pragma.html
https://cj.rs/blog/sqlite-pragma-cheatsheet-for-performance-and-consistency/
https://phiresky.github.io/blog/2020/sqlite-performance-tuning/