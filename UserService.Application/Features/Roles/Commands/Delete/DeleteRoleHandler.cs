using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Commands;

public class DeleteRoleHandler : DeleteBaseCommandHandler<Guid, DeleteRoleValidator, DeleteRoleDto, Role>
{
    public DeleteRoleHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ILogger<DeleteBaseCommandHandler<Guid, DeleteRoleValidator, DeleteRoleDto, Role>> pLogger,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pSharedResourceLocalizer)
        : base(pUnitOfWork, pMapper, pMediator, pLogger, pCurrentUserService, pSharedResourceLocalizer)
    {
    }
}
