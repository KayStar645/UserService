using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserService.Application.Features.Roles.Commands;
using UserService.Application.Features.Roles.Queries;

namespace UserService.API.Endpoints.Role;

public static partial class RoleEndpointExtensions
{
    private static readonly string _groupName = "/roles";
    public static RouteGroupBuilder MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup($"{_groupName}").WithTags("Roles");

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
        return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
    }

    private static async Task<IResult> HandleGetRole([AsParameters] GetRoleDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
    }


    private static async Task<IResult> HandleCreateRole([FromBody] CreateRoleDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Created($"{_groupName}/{result.Value.Id}", result) : Results.BadRequest(result);
    }

    private static async Task<IResult> HandleUpdateRole([Required] Guid id, [FromBody] UpdateRoleDto request, [FromServices] IMediator mediator)
    {
        request.Id = id;
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
    }

    private static async Task<IResult> HandleDeleteRole([AsParameters] DeleteRoleDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result);
    }
}

