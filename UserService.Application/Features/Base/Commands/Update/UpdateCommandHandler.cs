using Ardalis.Result;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using UserService.Application.Resources.Languages;
using UserService.Application.Services.Interface;
using UserService.Domain.Common.Entity;
using UserService.Domain.Events.Async;
using UserService.Domain.Events.FireAndForget;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Commands;


// Khi override lại phải luôn gọi lại phương thức virtual cha
public abstract class UpdateCommandHandler<TKey, TValidator, TRequest, TDto, TEntity> : IRequestHandler<TRequest, Result<TDto>>
        where TValidator : AbstractValidator<TRequest>
        where TRequest : UpdateCommandDto<TKey, TDto>, IRequest<Result<TDto>>
        where TEntity : BaseEntity<TKey>
{
    private readonly IUnitOfWork<TKey> _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly IMediator _mediator;
    protected readonly ICurrentUserService _currentUserService;
    protected readonly IStringLocalizer<LValidator> _validatorLocalizer;

    public UpdateCommandHandler(IUnitOfWork<TKey> pUnitOfWork, IMapper pMapper,
        IMediator pMediator, ICurrentUserService pCurrentUserService,
        IStringLocalizer<LValidator> pValidatorLocalizer)
    {
        _unitOfWork = pUnitOfWork;
        _mapper = pMapper;
        _mediator = pMediator;
        _currentUserService = pCurrentUserService;
        _validatorLocalizer = pValidatorLocalizer;
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

            var oldEntity = await HandlerBeforeUpdate(request);

            var updateResult = await HandlerUpdate(request, oldEntity, cancellationToken);

            await HandlerAfterUpdate(request, oldEntity, updateResult.entity, updateResult.dto);

            await EventAfterUpdate(request, oldEntity, updateResult.entity);

            await transaction.CommitAsync(cancellationToken);
            return updateResult.dto;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result<TDto>.Error(ex.Message);
        }
    }

    protected virtual async Task<Result<TDto>> Validator(TRequest request)
    {
        var validator = Activator.CreateInstance(typeof(TValidator), _unitOfWork, _validatorLocalizer) as TValidator;

        if (validator == null)
        {
            return Result<TDto>.Error(_validatorLocalizer["InternalServerError"]);
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

    protected virtual async Task<TEntity> HandlerBeforeUpdate(TRequest request)
    {
        var query = _unitOfWork.Set<TEntity>().GetById(request.Id);
        
        query = IncludeRelationsForUpdate(query);

        var findEntity = await query.SingleOrDefaultAsync();

        if (findEntity == null)
        {
            throw new ApplicationException(_validatorLocalizer["NameNotExistsValue", "Id", request?.Id?.ToString() ?? string.Empty]);
        }

        return findEntity;
    }

    protected virtual IQueryable<TEntity> IncludeRelationsForUpdate(IQueryable<TEntity> query)
    {
        return query;
    }

    protected virtual async Task<(Result<TDto> dto, TEntity entity)> HandlerUpdate(TRequest request, TEntity oldEntity, CancellationToken cancellationToken)
    {
        var newEntity = _mapper.Map<TEntity>(request);
        oldEntity.Update(newEntity);
        _unitOfWork.Set<TEntity>().Update(oldEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TDto>(newEntity);

        return (Result<TDto>.Success(dto), newEntity);
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    protected virtual async Task HandlerAfterUpdate(TRequest request, TEntity oldEntity, TEntity entity, TDto dto) { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    private async Task EventAfterUpdate(TRequest request, TEntity oldEntity, TEntity entity)
    {
        await _mediator.Publish(new UpdatedEventAsync<TRequest, TEntity>(request, oldEntity, entity));

        _ = _mediator.Publish(new UpdatedEventFireAndForget<TRequest, TEntity>(request, oldEntity, entity));
    }

}
