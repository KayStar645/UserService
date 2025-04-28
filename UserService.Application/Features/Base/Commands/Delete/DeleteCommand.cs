using Ardalis.Result;
using Ardalis.SharedKernel;

namespace UserService.Application.Features.Base.Commands.Delete;

public abstract record DeleteCommand<TKey> : ICommand<Result>
{
    public required TKey Id { get; set; }
}
