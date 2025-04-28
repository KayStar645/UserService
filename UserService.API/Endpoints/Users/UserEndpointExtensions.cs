namespace UserService.API.Endpoints.Users;

public static partial class UserEndpointExtensions
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users");


        return group;
    }
}
