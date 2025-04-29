using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using UserService.Application.Features.Base.Commands.Delete;
using UserService.Application.Features.Languages;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;
namespace UserService.Application.Features.Permissions.Commands.Delete;

public class DeletePermissionHandler : DeleteBaseCommandHandler<Guid, DeletePermissionValidator, DeletePermission, Permission>
{
    public DeletePermissionHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ICurrentUserService pCurrentUserService, IStringLocalizer<LValidator> pValidatorLocalizer)
        : base(pUnitOfWork, pMapper, pMediator, pCurrentUserService, pValidatorLocalizer)
    {
    }
}
