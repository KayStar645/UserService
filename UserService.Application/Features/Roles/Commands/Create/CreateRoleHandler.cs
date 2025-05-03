using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Commands;

public class CreateRoleHandler : CreateCommandHandler<Guid, CreateRoleValidator, CreateRoleDto, RoleDto, Role>
{
    public CreateRoleHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pSharedResourceLocalizer)
        : base(pUnitOfWork, pMapper, pMediator, pCurrentUserService, pSharedResourceLocalizer)
    {
    }
}
