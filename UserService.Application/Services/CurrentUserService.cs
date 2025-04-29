using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UserService.Application.Services.Interface;
using UserService.Domain.Enums;
using UserService.Infrastructure.Constants;

namespace UserService.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public ERole Role
    {
        get
        {
            var roleValue = _httpContextAccessor.HttpContext?.User?.FindFirstValue(CONSTANT_CLAIM_TYPES.ROLE);
            return Enum.TryParse<ERole>(roleValue, out var role) ? role : ERole.User;
        }
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(CONSTANT_CLAIM_TYPES.USER);
    public string? StaffId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(CONSTANT_CLAIM_TYPES.STAFF);
}
