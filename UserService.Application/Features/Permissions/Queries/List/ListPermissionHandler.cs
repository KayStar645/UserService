using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sieve.Services;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Queries;
using UserService.Application.Resources.Languages;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;
namespace UserService.Application.Features.Permissions.Queries.List;

public class ListPermissionHandler : ListQueryHandler<Guid, ListPermissionValidator, ListPermissionDto, PermissionDto, Permission>
{
    public ListPermissionHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService,
        IStringLocalizer<LValidator> pValidatorLocalizer, ISieveProcessor pSieveProcessor)
        : base(pUnitOfWork, pMapper, pMediator, pCurrentUserService, pValidatorLocalizer, pSieveProcessor)
    {
        _search = new[] { "Code", "Name" };
    }
}
