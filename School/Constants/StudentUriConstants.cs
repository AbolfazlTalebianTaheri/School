namespace School.Api.Constants
{
    public static class StudentUriConstants
    {
        private const string Controller = "students";
        public const string GetAll = $"{Controller}";
        public const string GetById = $"{Controller}/{{id}}";
        public const string Create = $"{Controller}";
        public const string Update = $"{Controller}/{{id}}";
        public const string Delete = $"{Controller}/{{id}}";
    }
}
