using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Queries;
using UserService.Application.Resources;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Queries;

public class GetRoleHandler : GetQueryHandler<Guid, GetRoleValidator, GetRoleDto, RoleDto, Role>
{
    public GetRoleHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ILogger<GetQueryHandler<Guid, GetRoleValidator, GetRoleDto, RoleDto, Role>> pLogger,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pSharedResourceLocalizer)
        : base(pUnitOfWork, pMapper, pMediator, pLogger, pCurrentUserService, pSharedResourceLocalizer)
    {

    }
}
