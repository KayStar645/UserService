namespace UserService.API.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseApplicationMiddlewares(this IApplicationBuilder app)
    {

        //app.UseMiddleware<AuthorizationMiddleware>();

        return app;
    }
}

