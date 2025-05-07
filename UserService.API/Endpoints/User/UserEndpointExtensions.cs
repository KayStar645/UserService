using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserService.Application.Features.Users.Commands;
using UserService.Application.Features.Users.Queries;
using ArdalisResult = Ardalis.Result;

namespace UserService.API.Endpoints.User;

public static partial class UserEndpointExtensions
{
    private static readonly string _groupName = "/users";

    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(_groupName).WithTags("Users");

        group.MapGet("/", HandleListUsers).WithSummary("Lấy danh sách users");
        group.MapGet("/{id:guid}", HandleGetUser).WithSummary("Chi tiết user");
        group.MapPost("/", HandleCreateUser).WithSummary("Tạo mới user");
        group.MapPut("/{id:guid}", HandleUpdateUser).WithSummary("Cập nhật user");
        group.MapPut("/change-password/{id:guid}", HandleChangePasswordUser).WithSummary("Cập nhật mật khẩu user");

        return group;
    }

    // Handler methods

    private static async Task<IResult> HandleListUsers([AsParameters] ListUserDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);

        return result.Status switch
        {
            ArdalisResult.ResultStatus.Ok => TypedResults.Ok(result.Value),
            ArdalisResult.ResultStatus.Invalid => TypedResults.BadRequest(result.ValidationErrors),
            ArdalisResult.ResultStatus.Error => TypedResults.Conflict(result.Errors),
            _ => TypedResults.Problem("Unexpected error", statusCode: 500)
        };
    }

    private static async Task<IResult> HandleGetUser([AsParameters] GetUserDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);

        return result.Status switch
        {
            ArdalisResult.ResultStatus.Ok => TypedResults.Ok(result.Value),
            ArdalisResult.ResultStatus.NotFound => TypedResults.NotFound(),
            ArdalisResult.ResultStatus.Invalid => TypedResults.BadRequest(result.ValidationErrors),
            ArdalisResult.ResultStatus.Error => TypedResults.Conflict(result.Errors),
            _ => TypedResults.Problem("Unexpected error", statusCode: 500)
        };
    }

    private static async Task<IResult> HandleCreateUser([FromBody] CreateUserDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);

        return result.Status switch
        {
            ArdalisResult.ResultStatus.Created => TypedResults.Created($"{_groupName}/{result.Value?.Id}", result.Value),
            ArdalisResult.ResultStatus.Invalid => TypedResults.BadRequest(result.ValidationErrors),
            ArdalisResult.ResultStatus.Error => TypedResults.BadRequest(result.Errors),
            _ => TypedResults.Problem("Unexpected error", statusCode: 500)
        };
    }

    private static async Task<IResult> HandleUpdateUser([Required] Guid id, [FromBody] UpdateUserDto request, [FromServices] IMediator mediator)
    {
        request.Id = id;
        var result = await mediator.Send(request);

        return result.Status switch
        {
            ArdalisResult.ResultStatus.Ok => TypedResults.Ok(result.Value),
            ArdalisResult.ResultStatus.NotFound => TypedResults.NotFound(),
            ArdalisResult.ResultStatus.Invalid => TypedResults.BadRequest(result.ValidationErrors),
            ArdalisResult.ResultStatus.Error => TypedResults.BadRequest(result.Errors),
            _ => TypedResults.Problem("Unexpected error", statusCode: 500)
        };
    }

    private static async Task<IResult> HandleChangePasswordUser([Required] Guid id, [FromBody] ChangePasswordDto request, [FromServices] IMediator mediator)
    {
        request.Id = id;
        var result = await mediator.Send(request);

        return result.Status switch
        {
            ArdalisResult.ResultStatus.Ok => TypedResults.Ok(result.Value),
            ArdalisResult.ResultStatus.NotFound => TypedResults.NotFound(),
            ArdalisResult.ResultStatus.Invalid => TypedResults.BadRequest(result.ValidationErrors),
            ArdalisResult.ResultStatus.Error => TypedResults.BadRequest(result.Errors),
            _ => TypedResults.Problem("Unexpected error", statusCode: 500)
        };
    }
}
