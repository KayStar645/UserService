using Microsoft.Extensions.Localization;
using UserService.Domain.DTOs;
using UserService.Application.Features.Base.Queries;
using UserService.Application.Resources;
using UserService.Domain.Common.Entity.Interfaces;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Users.Queries;

public record ListUserDto : ListQueryDto<UserDto>, IOrganizationScope
{
    public string? CompanyId { get; set; }
    public string? BranchId { get; set; }
}

public class ListUserValidator : ListQueryValidator<Guid, ListUserDto, UserDto>
{
    public ListUserValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedResourceLocalizer)
        : base(pUnitOfWork, pSharedResourceLocalizer)
    { }
}
