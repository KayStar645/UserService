using UserService.Domain.Common.Entity;

namespace UserService.Domain.Entities;

public record Role : SoftDeleteAuditableEntity<Guid>
{
    public string? Code { get; set; }
    public string? Name { get; set; }

    public virtual required ICollection<RolePermission> RolePermissions { get; set; }

    public override void Update(BaseEntity<Guid> entity)
    {
        base.Update(entity);

        if (entity is Role role)
        {
            Code = role.Code;
            Name = role.Name;
            
            RolePermissions.ToList().ForEach(x => x.IsRemoved = true);
            role.RolePermissions.ToList().ForEach(x => RolePermissions.Add(x));
        }
    }

}
