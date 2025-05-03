using Microsoft.Extensions.Localization;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Commands;

public record DeleteRoleDto : DeleteCommandDto<Guid>
{
}

public class DeleteRoleValidator : DeleteCommandValidator<Guid, DeleteRoleDto>
{
    public DeleteRoleValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedResourceLocalizer)
        : base(pUnitOfWork, pSharedResourceLocalizer)
    {

    }
}
