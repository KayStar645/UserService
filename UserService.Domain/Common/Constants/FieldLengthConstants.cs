namespace UserService.Domain.Common.Constants;

public static class FieldLengthConstants
{
    public const int IdMaxLength = 36;
    public const int CompanyIdMaxLength = 36;
    public const int BranchIdMaxLength = 36;
    public const int CodeMaxLength = 50;
    public const int NameMaxLength = 200;

    public const int EmailMaxLength = 50;
    public const int PhoneNumberMaxLength = 15;
    public const int UrlMaxLength = 200;
    public const int ExplainMaxLength = 500;

    public static class User
    {
        public const int PasswordHashMaxLength = 36;
        public const int UsernameMaxLength = 36;
        public const int FullNameMaxLength = 190;
    }
}
