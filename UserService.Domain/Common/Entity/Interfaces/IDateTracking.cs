namespace UserService.Domain.Common.Entity.Interfaces;

public interface IDateTracking
{
    DateTimeOffset? CreatedAt { get; set; }
    DateTimeOffset? LastModifiedAt { get; set; }
}
