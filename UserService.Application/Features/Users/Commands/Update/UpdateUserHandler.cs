using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using UserService.Domain.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Application.Services.Interface;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Users.Commands;

public class UpdateUserHandler : UpdateCommandHandler<Guid, UpdateUserValidator, UpdateUserDto, UserDto, User>
{
    public UpdateUserHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ILogger<UpdateCommandHandler<Guid, UpdateUserValidator, UpdateUserDto, UserDto, User>> pLogger,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pSharedResourceLocalizer)
        : base(pUnitOfWork, pMapper, pMediator, pLogger, pCurrentUserService, pSharedResourceLocalizer)
    {

    }
}
