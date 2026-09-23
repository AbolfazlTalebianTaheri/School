using School.Domain.Constants;

namespace School.Api.Validators.Messages
{
    public static class UserValidationMessages
    {
        public const string StudentIdRequiredForParent = "کد دانش آموزی برای والدین اجباری است!!";
        public static readonly string PasswordMinimumLength = $"پسورد نمی تواند کمتر از {UserConstant.MinimumLenghPassword} باشد";
        public static readonly string PasswordMaxLength = $"پسورد نمی تواند بیشتر از {UserConstant.MaxLenghPassword} باشد";
        public const string AnUppercaseLetterIsMandatory = "یک حرف بزگ اجباریست";
        public const string A_NumberIsRequired = "یک عدد اجباریست";
        public const string PhoneNumberRequired = "شماره تلفن اجباریست";
        public const string PhoneNumberInvalid = "شماره تلفن معتبر نیست";
        public const string UserNameRequired = "نام کاربری اجباریست";
        public static readonly string UserNameMaxLength = $"نام کاربری حداکثر باید {UserConstant.MaxLengthUserNameResponse} کاراکتر باشد.";
        public static readonly string UserNameMinimumLength = $"نام کاربری حداقل باید {UserConstant.MinimumUserNameResponse} کاراکتر باشد.";

    }
}
