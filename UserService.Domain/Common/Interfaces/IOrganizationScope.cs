namespace UserService.Domain.Common.Interfaces;

public interface IOrganizationScope
{
    string CompanyId { get; set; }
    string BranchId { get; set; }
}
