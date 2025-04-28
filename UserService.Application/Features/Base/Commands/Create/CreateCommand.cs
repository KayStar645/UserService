using Ardalis.Result;
using Ardalis.SharedKernel;

namespace UserService.Application.Features.Base.Commands.Create;

public abstract record CreateCommand<TDto> : ICommand<Result<TDto>>;