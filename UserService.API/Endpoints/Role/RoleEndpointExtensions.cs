namespace UserService.API.Endpoints.Role;

public static partial class RoleEndpointExtensions
{
    public static RouteGroupBuilder MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/roles");


        return group;
    }
}

