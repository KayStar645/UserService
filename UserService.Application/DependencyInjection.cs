using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using System.Reflection;
using UserService.Application.Profiles;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;

namespace UserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddSingleton(provider => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CommonMappingProfile());
            cfg.AddProfile(new ModuleMappingProfile());
        }).CreateMapper());

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<ISieveProcessor, SieveProcessor>();

        return services;
    }
}
