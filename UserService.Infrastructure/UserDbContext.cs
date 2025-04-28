using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserService.Domain.Entities;

namespace UserService.Infrastructure;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> User => Set<User>();
    public DbSet<Role> Role => Set<Role>();
    public DbSet<Permission> Permission => Set<Permission>();
    public DbSet<UserRole> UserRole => Set<UserRole>();
    public DbSet<RolePermission> RolePermission => Set<RolePermission>();
    public DbSet<UserPermission> UserPermission => Set<UserPermission>();


}
