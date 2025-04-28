using UserService.Domain.Common.Entity;

namespace UserService.Domain.Entities;

public record Permission : EntityAuditWithOrgDetail<Guid>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
}
