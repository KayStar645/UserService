using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Application.Services.Interface;
using UserService.Domain.DTOs;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Users.Commands;

public class ChangePasswordHandler : UpdateCommandHandler<Guid, ChangePasswordValidator, ChangePasswordDto, UserDto, User>
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public ChangePasswordHandler(IUnitOfWork<Guid> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ILogger<UpdateCommandHandler<Guid, ChangePasswordValidator, ChangePasswordDto, UserDto, User>> pLogger,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pSharedLocalizer,
        IPasswordHasher<User> pPasswordHasher)
        : base(pUnitOfWork, pMapper, pMediator, pLogger, pCurrentUserService, pSharedLocalizer)
    {
        _passwordHasher = pPasswordHasher;
    }

    protected override async Task<(User, ChangePasswordDto)> HandlerBeforeUpdate(ChangePasswordDto request)
    {
        var (oldEntity, newRequest) = await base.HandlerBeforeUpdate(request);

        var result = _passwordHasher.VerifyHashedPassword(oldEntity, oldEntity.PasswordHash, newRequest.OldPassword ?? string.Empty);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new ApplicationException(_sharedLocalizer["IncorrectPassword"]);
        }

        return (oldEntity, newRequest);
    }

    protected override void Update(User oldEntity, ChangePasswordDto request)
    {
        oldEntity.PasswordHash = _passwordHasher.HashPassword(oldEntity, request.NewPassword);
    }
}
