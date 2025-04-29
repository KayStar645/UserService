using Ardalis.Result;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using Sieve.Models;
using Sieve.Services;
using System.Linq.Expressions;
using UserService.Application.Features.Languages;
using UserService.Application.Services.Interface;
using UserService.Domain.Common.Entity;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Queries.List;


// Khi override lại phải luôn gọi lại phương thức virtual cha
public abstract class ListQueryHandler<TKey, TValidator, TRequest, TDto, TEntity> : IRequestHandler<TRequest, Result<PagedListResult<TDto>>>
        where TValidator : AbstractValidator<TRequest>
        where TRequest : ListQuery<TDto>, IRequest<Result<PagedListResult<TDto>>>
        where TEntity : BaseEntity<TKey>
{
    private readonly IUnitOfWork<TKey> _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly IMediator _mediator;
    protected readonly ICurrentUserService _currentUserService;
    protected readonly IStringLocalizer<LValidator> _validatorLocalizer;
    protected readonly ISieveProcessor _sieveProcessor;

    protected List<string> _fields = new List<string>();
    protected List<string> _search = new List<string>();

    public ListQueryHandler(IUnitOfWork<TKey> pUnitOfWork, IMapper pMapper,
        IMediator pMediator, ICurrentUserService pCurrentUserService,
        IStringLocalizer<LValidator> pLalidatorLocalizer,
        ISieveProcessor pSieveProcessor)
    {
        _unitOfWork = pUnitOfWork;
        _mapper = pMapper;
        _mediator = pMediator;
        _currentUserService = pCurrentUserService;
        _validatorLocalizer = pLalidatorLocalizer;
        _sieveProcessor = pSieveProcessor;
    }

    public virtual async Task<Result<PagedListResult<TDto>>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validatorResult = await Validator(request);
            if (!validatorResult.IsSuccess)
            {
                var errorMessage = string.Join(", ", validatorResult.Errors.Select(e => e));
                return Result<PagedListResult<TDto>>.Error(errorMessage);
            }

            var (result, listEntity) = await HandlerList(request, cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            return Result<PagedListResult<TDto>>.Error(ex.Message);
        }
    }

    protected virtual async Task<Result<PagedListResult<TDto>>> Validator(TRequest request)
    {
        var validator = Activator.CreateInstance(typeof(TValidator), _unitOfWork, _validatorLocalizer) as TValidator;

        if (validator == null)
        {
            return Result<PagedListResult<TDto>>.Error(_validatorLocalizer["InternalServerError"]);
        }

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors
                .Select(e => new ValidationError
                {
                    Identifier = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                })
                .ToList();

            return Result<PagedListResult<TDto>>.Invalid(validationErrors);
        }

        return Result<PagedListResult<TDto>>.Success(new PagedListResult<TDto>(Enumerable.Empty<TDto>(), 0, request.Page, 0));
    }



    protected virtual async Task<(Result<PagedListResult<TDto>> result, IEnumerable<TEntity> listEntity)> HandlerList(TRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Set<TEntity>().GetAll();

        var sieve = _mapper.Map<SieveModel>(request);
        query = ApplySelected(request, query);
        query = _sieveProcessor.Apply(sieve, query);
        query = ApplySearch(request, query);
        query = ApplyQuery(request, query);

        var results = await query.GetPagedDataAsync(request.Page, request.PageSize);
        var mapResults = _mapper.Map<PagedListResult<TDto>>(results);

        return (Result.Success(mapResults), results.Items);
    }

    protected virtual IQueryable<TEntity> ApplyQuery(TRequest request, IQueryable<TEntity> query)
    {
        return query;
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    protected virtual async Task<IEnumerable<TDto>> HandlerDtoListQuery(TRequest request, IEnumerable<TDto> listDto)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return listDto;
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

    #region Tìm kiếm
    private IQueryable<TEntity> ApplySearch(TRequest request, IQueryable<TEntity> query)
    {
        if (string.IsNullOrEmpty(request.Search))
            return query;

        var searchFields = _search;

        foreach (var field in searchFields)
        {
            if (field.Contains("."))
            {
                query = ApplySearchForNestedField(request, query, field);
            }
            else
            {
                query = ApplySearchForField(request, query, field);
            }
        }

        return query;
    }

    private IQueryable<TEntity> ApplySearchForField(TRequest request, IQueryable<TEntity> query, string field)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, field);

        if (property.Type == typeof(string))
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var searchExpression = Expression.Call(
                property,
                typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                Expression.Constant(request.Search)
            );
#pragma warning restore CS8604 // Possible null reference argument.

            var lambda = Expression.Lambda<Func<TEntity, bool>>(searchExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    private IQueryable<TEntity> ApplySearchForNestedField(TRequest request, IQueryable<TEntity> query, string field)
    {
        var parts = field.Split('.');

        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var propertyExpression = GetNestedPropertyExpression(parameter, parts);

        if (propertyExpression != null)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var searchExpression = Expression.Call(
                propertyExpression,
                typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                Expression.Constant(request.Search)
            );
#pragma warning restore CS8604 // Possible null reference argument.

            var lambda = Expression.Lambda<Func<TEntity, bool>>(searchExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    private Expression GetNestedPropertyExpression(Expression parameter, string[] parts)
    {
        Expression propertyExpression = parameter;

        foreach (var part in parts)
        {
            propertyExpression = Expression.Property(propertyExpression, part);
        }

        return propertyExpression;
    }
    #endregion
}
