﻿using Ardalis.Result;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using UserService.Application.Resources;
using UserService.Application.Services.Interface;
using UserService.Domain.Common.Entity;
using UserService.Domain.Events.Async;
using UserService.Domain.Events.FireAndForget;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Commands;

// Khi override lại phải luôn gọi lại phương thức virtual cha
public abstract class DeleteBaseCommandHandler<TKey, TValidator, TRequest, TEntity> : IRequestHandler<TRequest, Result>
    where TValidator : AbstractValidator<TRequest>
    where TRequest : DeleteCommandDto<TKey>, IRequest<Result>
    where TEntity : BaseEntity<TKey>
{
    private readonly IUnitOfWork<TKey> _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly IMediator _mediator;
    protected readonly ILogger<DeleteBaseCommandHandler<TKey, TValidator, TRequest, TEntity>> _logger;
    protected readonly ICurrentUserService _currentUserService;
    protected readonly IStringLocalizer<SharedResource> _sharedLocalizer;

    public DeleteBaseCommandHandler(IUnitOfWork<TKey> pUnitOfWork, IMapper pMapper, IMediator pMediator,
        ILogger<DeleteBaseCommandHandler<TKey, TValidator, TRequest, TEntity>> pLogger,
        ICurrentUserService pCurrentUserService, IStringLocalizer<SharedResource> pSharedLocalizer)
    {
        _unitOfWork = pUnitOfWork;
        _mapper = pMapper;
        _mediator = pMediator;
        _logger = pLogger;
        _currentUserService = pCurrentUserService;
        _sharedLocalizer = pSharedLocalizer;
    }

    public virtual async Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("\nBEGIN: {HandlerName}\n", GetType().Name);

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var validatorResult = await Validator(request);
            if (!validatorResult.IsSuccess)
            {
                return validatorResult;
            }

            await HandlerBeforeDelete(request);

            var entity = await Delete(request, cancellationToken);

            await HandlerAfterDelete(request);

            await EventAfterDelete(request, entity);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("\nEND: {HandlerName}\n", GetType().Name);
            return Result.NoContent();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Error(ex.Message);
        }
    }

    protected virtual async Task<Result> Validator(TRequest request)
    {
        var validator = Activator.CreateInstance(typeof(TValidator), _unitOfWork, _sharedLocalizer) as TValidator;

        if (validator == null)
        {
            return Result.Error(_sharedLocalizer["InternalServerError"]);
        }

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

            return Result.Error(new ErrorList(errorMessages));
        }

        return Result.Success();
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    protected virtual async Task HandlerBeforeDelete(TRequest request) { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously


    protected virtual async Task<TEntity> Delete(TRequest request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

        if (entity == null)
            throw new ApplicationException(_sharedLocalizer["NameNotExistsValue", nameof(request.Id), request?.Id?.ToString() ?? string.Empty]);

        _unitOfWork.Set<TEntity>().Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity;
    }


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    protected virtual async Task HandlerAfterDelete(TRequest request) { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    private async Task EventAfterDelete(TRequest request, TEntity entity)
    {
        await _mediator.Publish(new DeletedEventAsync<TRequest, TEntity>(request, entity));

        _ = _mediator.Publish(new DeletedEventFireAndForget<TRequest, TEntity>(request, entity));
    }
}