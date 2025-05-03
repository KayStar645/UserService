using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Sieve.Services;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Queries;
using UserService.Application.Resources;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Queries;

public class ListRoleHandler : ListQueryHandler<Guid, ListRoleValidator, ListRoleDto, RoleDto, Role>
{
    public ListRoleHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ILogger<ListQueryHandler<Guid, ListRoleValidator, ListRoleDto, RoleDto, Role>> pLogger,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pSharedResourceLocalizer, ISieveProcessor pSieveProcessor)
        : base(pUnitOfWork, pMapper, pMediator, pLogger, pCurrentUserService, pSharedResourceLocalizer, pSieveProcessor)
    {
        _search = new[] { "Code", "Name" };
    }
}
