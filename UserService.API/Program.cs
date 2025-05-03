using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using UserService.API.Endpoints;
using UserService.Application;
using UserService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Đăng ký các dịch vụ vào DI container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký các services từ Auth.Infrastructure và Auth.Application
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Cấu hình Swagger
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("vi"), new CultureInfo("en") };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("vi"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizeOptions.Value);

// Cấu hình các middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.RegisterAllEndpoints();

app.Run();