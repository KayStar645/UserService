using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources.Languages;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Commands;

public class UpdateRoleHandler : UpdateCommandHandler<Guid, UpdateRoleValidator, UpdateRoleDto, RoleDto, Role>
{
    public UpdateRoleHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ICurrentUserService pCurrentUserService, IStringLocalizer<LValidator> pValidatorLocalizer)
        : base(pUnitOfWork, pMapper, pMediator, pCurrentUserService, pValidatorLocalizer)
    {

    }
}
