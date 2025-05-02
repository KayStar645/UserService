# UserService API

Add-Migration InitialCreate -Project UserService.Infrastructure -StartupProject UserService.API -Context UserDbContext

Remove-Migration -Project UserService.Infrastructure -StartupProject UserService.API

Update-Database -Project UserService.Infrastructure -StartupProject UserService.API -Context UserDbContext

Get-Migration -Project UserService.Infrastructure -StartupProject UserService.API -Context UserDbContext


** Đối với PostgreSQL
```
	CREATE EXTENSION IF NOT EXISTS unaccent;
```