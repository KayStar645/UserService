using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Permissions.Commands;
using UserService.Application.Features.Permissions.Queries;
using ArdalisResult = Ardalis.Result;

namespace UserService.API.Endpoints.Permission;

public static partial class PermissionEndpointExtensions
{
    private static readonly string _groupName = "/permissions";

    public static RouteGroupBuilder MapPermissionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(_groupName).WithTags("Permissions");

        group.MapGet("/", HandleListPermissions).WithSummary("Lấy danh sách permissions");
        group.MapPost("/", HandleCreatePermission).WithSummary("Tạo mới permission");
        group.MapDelete("/{id:guid}", HandleDeletePermission).WithSummary("Xóa permission theo ID");

        return group;
    }

    // Handler methods

    private static async Task<IResult> HandleListPermissions([AsParameters] ListPermissionDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);

        return result.Status switch
        {
            ArdalisResult.ResultStatus.Ok => TypedResults.Ok(result.Value),
            ArdalisResult.ResultStatus.Invalid => TypedResults.BadRequest(result.ValidationErrors),
            _ => TypedResults.Problem("Unexpected error", statusCode: 500)
        };
    }

    private static async Task<IResult> HandleCreatePermission([FromBody] CreatePermissionDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);

        return result.Status switch
        {
            ArdalisResult.ResultStatus.Created => TypedResults.Created($"{_groupName}/{result.Value?.Id}", result.Value),
            ArdalisResult.ResultStatus.Invalid => TypedResults.BadRequest(result.ValidationErrors),
            _ => TypedResults.Problem("Unexpected error", statusCode: 500)
        };
    }

    private static async Task<IResult> HandleDeletePermission([AsParameters] DeletePermissionDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);

        return result.Status switch
        {
            ArdalisResult.ResultStatus.NoContent => TypedResults.NoContent(),
            ArdalisResult.ResultStatus.NotFound => TypedResults.NotFound(),
            _ => TypedResults.Problem("Unexpected error", statusCode: 500)
        };
    }
}
