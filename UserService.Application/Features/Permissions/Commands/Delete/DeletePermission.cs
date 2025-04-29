using Microsoft.Extensions.Localization;
using UserService.Application.Features.Base.Commands.Delete;
using UserService.Application.Features.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Permissions.Commands.Delete;

public record DeletePermission : DeleteCommand<Guid>
{
}

public class DeletePermissionValidator : DeleteCommandValidator<Guid, DeletePermission>
{
    public DeletePermissionValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
        : base(pUnitOfWork, pValidatorLocalizer)
    {
        
    }
}
