using UserService.Domain.Enums;

namespace UserService.Domain.Common.Interfaces;

public interface IStatus
{
    EStatus Status { get; set; }
}
