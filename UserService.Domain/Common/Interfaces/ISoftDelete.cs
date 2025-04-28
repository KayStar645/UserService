namespace UserService.Domain.Common.Interfaces;

public interface ISoftDelete
{
    bool IsRemoved { get; set; }
}
