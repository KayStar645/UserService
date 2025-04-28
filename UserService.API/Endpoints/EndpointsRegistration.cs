using UserService.API.Endpoints.Roles;
using UserService.API.Endpoints.Users;

namespace UserService.API.Endpoints;

public static class EndpointsRegistration
{
    public static void RegisterAllEndpoints(this WebApplication app)
    {
        app.MapUserEndpoints();
        app.MapRoleEndpoints();
    }
}
