using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Sieve.Services;
using UserService.Domain.DTOs;
using UserService.Application.Features.Base.Queries;
using UserService.Application.Resources;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Users.Queries;

public class ListUserHandler : ListQueryHandler<Guid, ListUserValidator, ListUserDto, UserDto, User>
{
    public ListUserHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ILogger<ListQueryHandler<Guid, ListUserValidator, ListUserDto, UserDto, User>> pLogger,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pSharedLocalizer, ISieveProcessor pSieveProcessor)
        : base(pUnitOfWork, pMapper, pMediator, pLogger, pCurrentUserService, pSharedLocalizer, pSieveProcessor)
    {
        _search = new[] { nameof(User.Username), nameof(User.Email), nameof(User.PhoneNumber), nameof(User.FullName) };

    }
}
