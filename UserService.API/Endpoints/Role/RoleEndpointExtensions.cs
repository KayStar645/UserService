using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserService.Application.Features.Roles.Commands;
using UserService.Application.Features.Roles.Queries;

namespace UserService.API.Endpoints.Role;

public static partial class RoleEndpointExtensions
{
    public static RouteGroupBuilder MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/roles").WithTags("Roles");

        group.MapGet("/", HandleListRoles).WithSummary("Lấy danh sách roles");
        group.MapGet("/{id:guid}", HandleGetRole).WithSummary("Chi tiết role");
        group.MapPost("/", HandleCreateRole).WithSummary("Tạo mới role");
        group.MapPut("/{id:guid}", HandleUpdateRole).WithSummary("Cập nhật role");
        group.MapDelete("/{id:guid}", HandleDeleteRole).WithSummary("Xóa role theo ID");

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


    private static async Task<IResult> HandleCreateRole([FromBody] CreateRoleDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> HandleUpdateRole([Required] Guid id, [FromBody] UpdateRoleDto request, [FromServices] IMediator mediator)
    {
        request.Id = id;
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> HandleDeleteRole([AsParameters] DeleteRoleDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
    }
}

