using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Commands;

public class UpdateRoleHandler : UpdateCommandHandler<Guid, UpdateRoleValidator, UpdateRoleDto, RoleDto, Role>
{
    public UpdateRoleHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pSharedLocalizer)
        : base(pUnitOfWork, pMapper, pMediator, pCurrentUserService, pSharedLocalizer)
    {

    }

    protected override IQueryable<Role> IncludeRelationsForUpdate(IQueryable<Role> query)
    {
        return query.Include(x => x.RolePermissions);
    }
}
