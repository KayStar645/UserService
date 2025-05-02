using Ardalis.Result;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Sieve.Models;
using Sieve.Services;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using UserService.Application.Resources.Languages;
using UserService.Application.Services.Interface;
using UserService.Domain.Common.Entity;
using UserService.Infrastructure.Common;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Queries;


// Khi override lại phải luôn gọi lại phương thức virtual cha
public abstract class ListQueryHandler<TKey, TValidator, TRequest, TDto, TEntity> : IRequestHandler<TRequest, Result<PagedListResult<TDto>>>
        where TValidator : AbstractValidator<TRequest>
        where TRequest : ListQueryDto<TDto>, IRequest<Result<PagedListResult<TDto>>>
        where TEntity : BaseEntity<TKey>
{
    private readonly IUnitOfWork<TKey> _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly IMediator _mediator;
    protected readonly ICurrentUserService _currentUserService;
    protected readonly IStringLocalizer<SharedResource> _validatorLocalizer;
    protected readonly ISieveProcessor _sieveProcessor;

    protected string[] _fields = Array.Empty<string>();
    protected string[] _search = Array.Empty<string>();

    public ListQueryHandler(IUnitOfWork<TKey> pUnitOfWork, IMapper pMapper,
        IMediator pMediator, ICurrentUserService pCurrentUserService,
        IStringLocalizer<SharedResource> pValidatorLocalizer,
        ISieveProcessor pSieveProcessor)
    {
        _unitOfWork = pUnitOfWork;
        _mapper = pMapper;
        _mediator = pMediator;
        _currentUserService = pCurrentUserService;
        _validatorLocalizer = pValidatorLocalizer;
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

        query = ApplyIncludes(request, query);
        query = ApplySelected(request, query);

        var noPagingSieve = new SieveModel { Filters = request.Filters, Sorts = request.Sorts };
        query = _sieveProcessor.Apply(noPagingSieve, query);

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

    #region Fields - Select những trường nào
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

    #region Search - Tìm kiếm theo gì
    private IQueryable<TEntity> ApplySearch(TRequest request, IQueryable<TEntity> query)
    {
        if (string.IsNullOrWhiteSpace(request.Search))
            return query;

        var parameter = Expression.Parameter(typeof(TEntity), "x");
        Expression? orExpression = null;

        foreach (var field in _search)
        {
            Expression? propertyExpression;

            if (field.Contains('.'))
            {
                var parts = field.Split('.');
                propertyExpression = GetNestedPropertyExpression(parameter, parts);
            }
            else
            {
                propertyExpression = Expression.Property(parameter, field);
            }

            if (propertyExpression != null && propertyExpression.Type == typeof(string))
            {
                var expr = BuildCaseInsensitiveContains(propertyExpression, request.Search!);
                orExpression = orExpression == null ? expr : Expression.OrElse(orExpression, expr);
            }
        }

        if (orExpression == null)
            return query;

        var lambda = Expression.Lambda<Func<TEntity, bool>>(orExpression, parameter);
        return query.Where(lambda);
    }

    private IQueryable<TEntity> ApplySearchForField(TRequest request, IQueryable<TEntity> query, string field)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, field);

        if (property.Type == typeof(string))
        {
            var searchExpression = BuildCaseInsensitiveContains(property, request.Search!);
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

        if (propertyExpression != null && propertyExpression.Type == typeof(string))
        {
            var searchExpression = BuildCaseInsensitiveContains(propertyExpression, request.Search!);
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

    private Expression BuildCaseInsensitiveContains(Expression propertyExpression, string searchValue)
    {
        var unaccentMethod = typeof(PgSqlDbFunctions).GetMethod(
            nameof(PgSqlDbFunctions.Unaccent),
            BindingFlags.Static | BindingFlags.Public
        ) ?? throw new InvalidOperationException("Unaccent method not found.");

        var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)
            ?? throw new InvalidOperationException("string.ToLower method not found.");

        // lower(unaccent(property))
        var unaccentedProperty = Expression.Call(null, unaccentMethod, propertyExpression);
        var loweredProperty = Expression.Call(unaccentedProperty, toLowerMethod);

        // lower(unaccent("search"))
        var searchExpr = Expression.Constant(searchValue, typeof(string));
        var unaccentedSearch = Expression.Call(null, unaccentMethod, searchExpr);
        var loweredSearch = Expression.Call(unaccentedSearch, toLowerMethod);

        // property.Contains(search)
        var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })
            ?? throw new InvalidOperationException("string.Contains method not found.");

        return Expression.Call(loweredProperty, containsMethod, loweredSearch);
    }

    #endregion

    #region Includes - Lấy thêm những obj nào
    private IQueryable<TEntity> ApplyIncludes(TRequest request, IQueryable<TEntity> query)
    {
        if (request.Includes == null || !request.Includes.Any())
            return query;

        foreach (var include in request.Includes)
        {
            query = ApplyInclude(query, include);
        }

        return query;
    }

    private static IQueryable<TEntity> ApplyInclude(IQueryable<TEntity> query, string includePath)
    {
        var props = includePath.Split('.');
        IQueryable<TEntity> result = query;

        if (props.Length == 1)
        {
            result = result.Include(props[0]);
        }
        else
        {
            string fullPath = string.Join('.', props);
            result = result.Include(fullPath);
        }

        return result;
    }

    #endregion
}