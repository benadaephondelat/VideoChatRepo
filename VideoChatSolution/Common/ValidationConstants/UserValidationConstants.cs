namespace Common.ValidationConstants
{
    public static class UserValidationConstants
    {
        public const bool DoesUserPasswordRequiresDigit = true;
        public const string UserPasswordRequiresDigitMessage = "Password requires at least one digit";

        public const bool DoesUserPasswordRequiresLowercase = true;
        public const string UserPasswordRequiresLowercaseMessage = "Password requires at least one lowercase character";

        public const bool DoesUserPasswordRequiresUppercase = true;
        public const string UserPasswordRequiresUpercaseMessage = "Password requires at least one upercase character";

        public const bool DoesUserPasswordRequiresSymbol = true;
        public const string DoesUserPasswordRequiresSymbolMessage = "Password must contain a symbol";

        public const int UserPasswordLength = 7;
        public const string UserPasswordLengthMessage = "User password must be at least 7 characters long";
    }
}
