namespace UserService.API.Endpoints.User;

public static partial class UserEndpointExtensions
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users");


        return group;
    }
}
