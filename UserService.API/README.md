# UserService API

Add-Migration InitialCreate -Project UserService.Infrastructure -StartupProject UserService.API -Context UserDbContext

Update-Database -Project UserService.Infrastructure -StartupProject UserService.API -Context UserDbContext