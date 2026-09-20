namespace School.Api.Constants
{
    public static class LessonUriContracts
    {
        private const string Controller = "lessons";
        public const string GetAll = $"{Controller}";
        public const string GetById = $"{Controller}/{{id}}";
        public const string Create = $"{Controller}";
        public const string Update = $"{Controller}/{{id}}";
        public const string Delete = $"{Controller}/{{id}}";
    }
}
