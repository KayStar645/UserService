using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;
namespace UserService.Application.Features.Permissions.Commands;

public class DeletePermissionHandler : DeleteBaseCommandHandler<Guid, DeletePermissionValidator, DeletePermissionDto, Permission>
{
    public DeletePermissionHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pSharedLocalizer)
        : base(pUnitOfWork, pMapper, pMediator, pCurrentUserService, pSharedLocalizer)
    {
    }
}
