using UserService.Application.Features.Base.Commands;
using UserService.Domain.DTOs;

namespace UserService.Application.Features.Users.Commands;

public record ChangePasswordDto : UpdateCommandDto<Guid, UserDto>
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}

public class ChangePasswordValidator
{

}
