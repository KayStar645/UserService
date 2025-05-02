using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Sieve.Services;
using System.Reflection;
using UserService.Application.Profiles;
using UserService.Application.Services;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;

namespace UserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
        services.AddScoped(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

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
