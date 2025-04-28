namespace UserService.Domain.Common.DTO;

public record DropdownDto<TKey>
{
    public required TKey Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }

    public DropdownDto()
    {
    }

    public DropdownDto(TKey id, string name)
    {
        Id = id;
        Name = name;
    }

    public DropdownDto(TKey id, string name, string code)
    {
        Id = id;
        Name = name;
        Code = code;
    }
}
