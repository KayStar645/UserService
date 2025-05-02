using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.Common;

public static class PgSqlDbFunctions
{
    [DbFunction("unaccent", IsBuiltIn = true)]
    public static string Unaccent(string input)
    {
        // Khi chạy LINQ-to-Entities, EF sẽ dịch sang SQL
        // Khi chạy LINQ-to-Objects (không dùng trong EF), trả về luôn input để tránh lỗi
        return input;
    }
}

