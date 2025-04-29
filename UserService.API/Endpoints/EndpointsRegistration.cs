using UserService.API.Endpoints.Permission;
using UserService.API.Endpoints.Role;
using UserService.API.Endpoints.User;

namespace UserService.API.Endpoints;

public static class EndpointsRegistration
{
    public static void RegisterAllEndpoints(this WebApplication app)
    {
        app.MapPermissionEndpoints();
        app.MapRoleEndpoints();
        app.MapUserEndpoints();
    }
}
