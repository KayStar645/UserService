using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Entities;
using UserService.Infrastructure.Interceptors;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<EntitySaveChangesInterceptor>();

        services.AddDbContext<UserDbContext>((serviceProvider, options) =>
        {
            var interceptor = serviceProvider.GetRequiredService<EntitySaveChangesInterceptor>();
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                   .AddInterceptors(interceptor);
        });

        services.AddScoped<UserDbContextInitializer>();
        //services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        //services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        return services;
    }
}
