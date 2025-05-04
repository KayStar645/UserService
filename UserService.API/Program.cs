using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using UserService.API.Endpoints;
using UserService.API.Middleware;
using UserService.API.Swagger;
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
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        c.OperationFilter<AcceptLanguageHeaderOperationFilter>();
    });

}

var app = builder.Build();

var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizeOptions.Value);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseHttpsRedirection();

app.UseApplicationMiddlewares();

app.RegisterAllEndpoints();

app.Run();