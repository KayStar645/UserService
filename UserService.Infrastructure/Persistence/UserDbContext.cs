using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserService.Domain.Entities;
using UserService.Infrastructure.Common;

namespace UserService.Infrastructure.Persistence;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.ConfigureCommonColumn();
        modelBuilder.ConfigureSharedColumnOrder();

        modelBuilder.HasDbFunction(() => PgSqlDbFunctions.Unaccent(default!));
    }

    public DbSet<User> User => Set<User>();
    public DbSet<Role> Role => Set<Role>();
    public DbSet<Permission> Permission => Set<Permission>();
    public DbSet<UserRole> UserRole => Set<UserRole>();
    public DbSet<RolePermission> RolePermission => Set<RolePermission>();
    public DbSet<UserPermission> UserPermission => Set<UserPermission>();


}
