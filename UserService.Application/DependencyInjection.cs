using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using UserService.Application.Profiles;
using UserService.Domain.Entities;

namespace UserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(provider => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CommonMappingProfile());
            cfg.AddProfile(new ModuleMappingProfile());
        }).CreateMapper());

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddScoped<ISieveProcessor, SieveProcessor>();

        return services;
    }
}
