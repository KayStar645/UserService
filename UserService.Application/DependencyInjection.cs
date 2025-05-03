using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using System.Globalization;
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
        // Đăng ký biến ngôn ngữ
        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(
            options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("vi-VN"),
                    new CultureInfo("en-US"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "vi-VN", uiCulture: "vi-VN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
            });

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
