using UserService.Domain.Enums;

namespace UserService.Application.Services.Interface;

public interface ICurrentUserService
{
    public Guid? UserId { get; }

    public ERole Role { get; }
}