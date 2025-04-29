using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Permissions.Commands.Create;
using UserService.Application.Features.Permissions.Commands.Delete;
using UserService.Application.Features.Permissions.Queries.List;

namespace UserService.API.Endpoints.Permission;

public static partial class PermissionEndpointExtensions
{
    public static RouteGroupBuilder MapPermissionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/permissions");

        group.MapGet("/", HandleListPermissions).WithSummary("Lấy danh sách permissions");

        group.MapPost("/create", HandleCreatePermission).WithSummary("Tạo mới permission");
        group.MapDelete("/delete/{id}", HandleDeletePermission).WithSummary("Xóa permission theo ID");



        return group;
    }

    // Handler methods

    private static async Task<IResult> HandleListPermissions([AsParameters] ListPermission request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
    }


    private static async Task<IResult> HandleCreatePermission(CreatePermission request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> HandleDeletePermission(Guid id, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new DeletePermission { Id = id });
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
    }
}

