using Ardalis.Result;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using UserService.Application.Features.Languages;
using UserService.Application.Services.Interface;
using UserService.Domain.Common.Entity;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Queries.Get;


// Khi override lại phải luôn gọi lại phương thức virtual cha
public abstract class GetQueryHandler<TKey, TValidator, TRequest, TDto, TEntity> : IRequestHandler<TRequest, Result<TDto>>
        where TValidator : AbstractValidator<TRequest>
        where TRequest : GetQuery<TKey, TDto>, IRequest<Result<TDto>>
        where TEntity : BaseEntity<TKey>
{
    private readonly IUnitOfWork<TKey> _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly IMediator _mediator;
    protected readonly ICurrentUserService _currentUserService;
    protected readonly IStringLocalizer<LValidator> _validatorLocalizer;

    protected List<string> _fields = new List<string>();

    public GetQueryHandler(IUnitOfWork<TKey> pUnitOfWork, IMapper pMapper,
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
        try
        {
            var validatorResult = await Validator(request);
            if (!validatorResult.IsSuccess)
            {
                return validatorResult;
            }

            var detail = await HandlerGet(request, cancellationToken);

            return detail.dto;
        }
        catch (Exception ex)
        {
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

    protected virtual async Task<(Result<TDto> dto, TEntity? entity)> HandlerGet(TRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Set<TEntity>().GetById(request.Id);

        query = ApplySelected(request, query);

        query = ApplyQuery(request, query);

        var findEntity = await query.FirstOrDefaultAsync();

        if (findEntity == null)
        {
            throw new ApplicationException(_validatorLocalizer["NameNotExistsValue", "Id", request.Id.ToString()]);
        }

        var mapDto = _mapper.Map<TDto>(findEntity);
        var dto = await HandlerDtoAfterQuery(mapDto);

        return (Result<TDto>.Success(dto), findEntity);
    }

    protected virtual IQueryable<TEntity> ApplyQuery(TRequest request, IQueryable<TEntity> query) { return query; }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    protected virtual async Task<TDto> HandlerDtoAfterQuery(TDto dto)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return dto;
    }

    #region Lấy trường truy vấn
    private IQueryable<TEntity> ApplySelected(TRequest request, IQueryable<TEntity> query)
    {
        if (request.Fields == null || !request.Fields.Any())
            return query;

        List<string> fieldsToSelect = request.Fields.Intersect(_fields).ToList();

        if (!fieldsToSelect.Any())
            return query;

        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var bindings = new List<MemberBinding>();

        foreach (var field in fieldsToSelect)
        {
            var property = typeof(TEntity).GetProperty(field);
            if (property != null)
            {
                var propertyExpression = Expression.Property(parameter, property);
                var binding = Expression.Bind(property, propertyExpression);
                bindings.Add(binding);
            }
        }

        var memberInitExpression = Expression.MemberInit(Expression.New(typeof(TEntity)), bindings);

        var lambda = Expression.Lambda<Func<TEntity, TEntity>>(memberInitExpression, parameter);
        query = query.Select(lambda);

        return query;
    }
    #endregion
}