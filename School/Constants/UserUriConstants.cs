namespace School.Api.Constants
{
    public class UserUriConstants
    {
        private const string Controller = "users";
        public const string GetAll = $"{Controller}";
        public const string GetById = $"{Controller}/{{id}}";
        public const string GetByPhoneNumber = $"{Controller}/{{phone}}/{{phoneNumber}}";
        public const string GetByRoleAndUserName = $"{Controller}/{{role}}/{{userName}}";
        public const string UserNameExists = $"{Controller}/username/exists/{{userName}}";
        public const string Create = $"{Controller}";
        public const string Update = $"{Controller}/{{id}}";
        public const string Delete = $"{Controller}/{{id}}";
    }
}
