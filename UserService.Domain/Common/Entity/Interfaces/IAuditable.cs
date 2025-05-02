namespace UserService.Domain.Common.Entity.Interfaces;

public interface IAuditable : IDateTracking
{
    string? CreatedByCode { get; set; }
    string? LastModifiedByCode { get; set; }
    string? CreatedByUser { get; set; }
    string? LastModifiedByUser { get; set; }
}
