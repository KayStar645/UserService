using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Permissions.Commands;
using UserService.Application.Features.Permissions.Queries;

namespace UserService.API.Endpoints.Permission;

public static partial class PermissionEndpointExtensions
{
    public static RouteGroupBuilder MapPermissionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/permissions");

        group.MapGet("/", HandleListPermissions).WithSummary("Lấy danh sách permissions");

        group.MapPost("/", HandleCreatePermission).WithSummary("Tạo mới permission");
        group.MapDelete("/{id:guid}", HandleDeletePermission).WithSummary("Xóa permission theo ID");



        return group;
    }

    // Handler methods

    private static async Task<IResult> HandleListPermissions([AsParameters] ListPermissionDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
    }


    private static async Task<IResult> HandleCreatePermission(CreatePermissionDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> HandleDeletePermission([AsParameters] DeletePermissionDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
    }
}

