using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Roles.Commands;
using UserService.Application.Features.Roles.Queries;

namespace UserService.API.Endpoints.Role;

public static partial class RoleEndpointExtensions
{
    public static RouteGroupBuilder MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/roles");

        group.MapGet("/", HandleListRoles).WithSummary("Lấy danh sách roles");
        group.MapGet("/{id:guid}", HandleGetRole).WithSummary("Chi tiết role");

        group.MapPost("/create", HandleCreateRole).WithSummary("Tạo mới role");
        group.MapDelete("/delete/{roleId:guid}", HandleDeleteRole).WithSummary("Xóa role theo ID");

        return group;
    }

    // Handler methods

    private static async Task<IResult> HandleListRoles([AsParameters] ListRoleDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> HandleGetRole([AsParameters] GetRoleDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
    }


    private static async Task<IResult> HandleCreateRole(CreateRoleDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> HandleDeleteRole(Guid roleId, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new DeleteRoleDto { Id = roleId });
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
    }
}

