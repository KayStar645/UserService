# UserService API

Add-Migration InitialCreate -Project UserService.Infrastructure -StartupProject UserService.API -Context UserDbContext

Remove-Migration -Project UserService.Infrastructure -StartupProject UserService.API

Update-Database -Project UserService.Infrastructure -StartupProject UserService.API -Context UserDbContext

Update-Database LastGoodMigrationName -Project UserService.Infrastructure -StartupProject UserService.API -Context UserDbContext
