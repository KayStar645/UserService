using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Domain.Entities;

namespace UserService.Infrastructure;

public class UserDbContextInitializer
{
    private readonly ILogger<UserDbContextInitializer> _logger;
    private readonly UserDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserDbContextInitializer(ILogger<UserDbContextInitializer> logger, UserDbContext context, IPasswordHasher<User> pPasswordHasher)
    {
        _logger = logger;
        _context = context;
        _passwordHasher = pPasswordHasher;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        //Default Admin
       

        //Default Role
        

        // Add role to admin
        

        // Add permission to role
        
    }
}
