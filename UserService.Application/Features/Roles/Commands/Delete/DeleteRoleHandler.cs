using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources.Languages;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Commands;

public class DeleteRoleHandler : DeleteBaseCommandHandler<Guid, DeleteRoleValidator, DeleteRoleDto, Role>
{
    public DeleteRoleHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pValidatorLocalizer)
        : base(pUnitOfWork, pMapper, pMediator, pCurrentUserService, pValidatorLocalizer)
    {
    }
}
