using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Commands.Create;
using UserService.Application.Features.Languages;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Permissions.Commands.Create;

public class CreatePermissionHandler : CreateCommandHandler<Guid, CreatePermissionValidator, CreatePermission, PermissionDto, Permission>
{
    public CreatePermissionHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ICurrentUserService pCurrentUserService, IStringLocalizer<LValidator> pValidatorLocalizer)
        :base(pUnitOfWork, pMapper, pMediator, pCurrentUserService, pValidatorLocalizer)
    {
    }
}
