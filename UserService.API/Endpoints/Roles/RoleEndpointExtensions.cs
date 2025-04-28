namespace UserService.API.Endpoints.Roles;

public static partial class RoleEndpointExtensions
{
    public static RouteGroupBuilder MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/roles");


        return group;
    }
}

