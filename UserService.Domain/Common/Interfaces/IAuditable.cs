namespace UserService.Domain.Common.Interfaces;

public interface IAuditable : IDateTracking
{
    string? CreatedByCode { get; set; }
    string? ModifiedByCode { get; set; }
    string? CreatedByUser { get; set; }
    string? ModifiedByUser { get; set; }
}
