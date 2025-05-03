using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources.Languages;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Permissions.Commands;

public class CreatePermissionHandler : CreateCommandHandler<Guid, CreatePermissionValidator, CreatePermissionDto, PermissionDto, Permission>
{
    public CreatePermissionHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pSharedLocalizer)
        :base(pUnitOfWork, pMapper, pMediator, pCurrentUserService, pSharedLocalizer)
    {
    }
}
