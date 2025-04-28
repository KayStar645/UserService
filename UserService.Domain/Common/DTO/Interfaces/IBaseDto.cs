using UserService.Domain.Enums;

namespace UserService.Domain.Common.DTO.Interfaces;

public interface IBaseDto<TKey>
{
    TKey? Id { get; set; }
}