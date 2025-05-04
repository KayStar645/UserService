using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace UserService.Infrastructure.Common.Converters;

public class UtcDateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
{
    public UtcDateTimeOffsetConverter()
        : base(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v.UtcDateTime, DateTimeKind.Utc))
    {
    }
}
