using UserService.Domain.Enums;

namespace UserService.Application.Services.Interface;

public interface ICurrentUserService
{
    public string? UserId { get; }
    public string? StaffId { get; }

    public ERole Role { get; }
}