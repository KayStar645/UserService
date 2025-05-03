using Ardalis.Result;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using UserService.Application.Resources.Languages;
using UserService.Application.Services.Interface;
using UserService.Domain.Common.Entity;
using UserService.Domain.Events.Async;
using UserService.Domain.Events.FireAndForget;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Commands;


// Khi override lại phải luôn gọi lại phương thức virtual cha
public abstract class CreateCommandHandler<TKey, TValidator, TRequest, TDto, TEntity> : IRequestHandler<TRequest, Result<TDto>>
    where TValidator : AbstractValidator<TRequest>
    where TRequest : CreateCommandDto<TDto>, IRequest<Result<TDto>>
    where TEntity : BaseEntity<TKey>
{
    private readonly IUnitOfWork<TKey> _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly IMediator _mediator;
    protected readonly ICurrentUserService _currentUserService;
    protected readonly IStringLocalizer<SharedResource> _sharedLocalizer;

    public CreateCommandHandler(IUnitOfWork<TKey> pUnitOfWork, IMapper pMapper,
        IMediator pMediator, ICurrentUserService pCurrentUserService,
         IStringLocalizer<SharedResource> validatorLocalizer)
    {
        _unitOfWork = pUnitOfWork;
        _mapper = pMapper;
        _mediator = pMediator;
        _currentUserService = pCurrentUserService;
        _sharedLocalizer = validatorLocalizer;
    }

    public virtual async Task<Result<TDto>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var validatorResult = await Validator(request);
            if (!validatorResult.IsSuccess)
            {
                return validatorResult;
            }

            var entity = await HandlerBeforeCreate(request);

            var createResult = await HandlerCreate(entity, cancellationToken);

            await HandlerAfterCreate(request, createResult.entity, createResult.dto);

            await EventAfterCreate(request, createResult.entity);

            await transaction.CommitAsync(cancellationToken);
            return createResult.dto;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result<TDto>.Error(ex.Message);
        }
    }

    protected virtual async Task<Result<TDto>> Validator(TRequest request)
    {
        var validator = Activator.CreateInstance(typeof(TValidator), _unitOfWork, _sharedLocalizer) as TValidator;

        if (validator == null)
        {
            return Result<TDto>.Error(_sharedLocalizer["InternalServerError"]);
        }

        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsValid == false)
        {
            var validationErrors = validationResult.Errors
            .Select(e => new ValidationError
            {
                Identifier = e.PropertyName,
                ErrorMessage = e.ErrorMessage
            })
            .ToList();

            return Result<TDto>.Invalid(validationErrors);
        }

        return Result<TDto>.Success(_mapper.Map<TDto>(request));
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    protected virtual async Task<TEntity> HandlerBeforeCreate(TRequest request)
    {
        return _mapper.Map<TEntity>(request);
    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    protected virtual async Task<(Result<TDto> dto, TEntity entity)> HandlerCreate(TEntity entity, CancellationToken cancellationToken)
    {
        await _unitOfWork.Set<TEntity>().AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TDto>(entity);

        return (Result<TDto>.Success(dto), entity);
    }


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    protected virtual async Task HandlerAfterCreate(TRequest request, TEntity entity, TDto dto) { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    private async Task EventAfterCreate(TRequest request, TEntity entity)
    {
        await _mediator.Publish(new CreatedEventAsync<TRequest, TEntity>(request, entity));

        _ = _mediator.Publish(new CreatedEventFireAndForget<TRequest, TEntity>(request, entity));
    }
}
