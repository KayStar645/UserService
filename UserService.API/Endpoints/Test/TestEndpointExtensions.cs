using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using UserService.Application.Resources;

namespace UserService.API.Endpoints.Test;

public static partial class TestEndpointExtensions
{
    public static RouteGroupBuilder MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tests").WithTags("Tests");

        group.MapGet("/localizer", HandleGreeting).WithSummary("Kiểm tra biến dịch NameNotExistsValue");
        //group.MapGet("/localizer?culture=en-US", HandleGreeting).WithSummary("Kiểm tra biến dịch NameNotExistsValue");

        return group;
    }

    // Handler method
    private static async Task<IResult> HandleGreeting(
        [FromServices] IStringLocalizer<SharedResource> localizer)
    {
        var localizedString = localizer["NameNotExistsValue"];


        return Results.Ok(localizedString.Value);
    }
}


