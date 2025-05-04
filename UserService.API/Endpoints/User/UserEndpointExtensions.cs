using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserService.Application.Features.Users.Commands;
using UserService.Application.Features.Users.Queries;

namespace UserService.API.Endpoints.User;

public static partial class UserEndpointExtensions
{
    private static readonly string _groupName = "/users";
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup($"{_groupName}").WithTags("Users");

        group.MapGet("/", HandleListUsers).WithSummary("Lấy danh sách users");
        group.MapGet("/{id:guid}", HandleGetUser).WithSummary("Chi tiết user");
        group.MapPost("/", HandleCreateUser).WithSummary("Tạo mới user");
        group.MapPut("/{id:guid}", HandleUpdateUser).WithSummary("Cập nhật user");

        return group;
    }

    // Handler methods

    private static async Task<IResult> HandleListUsers([AsParameters] ListUserDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
    }

    private static async Task<IResult> HandleGetUser([AsParameters] GetUserDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
    }


    private static async Task<IResult> HandleCreateUser([FromBody] CreateUserDto request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Created($"{_groupName}/{result.Value.Id}", result) : Results.BadRequest(result);
    }

    private static async Task<IResult> HandleUpdateUser([Required] Guid id, [FromBody] UpdateUserDto request, [FromServices] IMediator mediator)
    {
        request.Id = id;
        var result = await mediator.Send(request);
        return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
    }
}
