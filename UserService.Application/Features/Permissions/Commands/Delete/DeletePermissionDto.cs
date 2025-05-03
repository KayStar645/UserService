using Microsoft.Extensions.Localization;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Permissions.Commands;

public record DeletePermissionDto : DeleteCommandDto<Guid>
{
}

public class DeletePermissionValidator : DeleteCommandValidator<Guid, DeletePermissionDto>
{
    public DeletePermissionValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedLocalizer)
        : base(pUnitOfWork, pSharedLocalizer)
    {
        
    }
}
