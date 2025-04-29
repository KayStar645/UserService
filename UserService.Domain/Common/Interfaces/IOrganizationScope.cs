namespace UserService.Domain.Common.Entity.Interfaces;

public interface IOrganizationScope
{
    string? CompanyId { get; set; }
    string? BranchId { get; set; }
}
