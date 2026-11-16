namespace School.Api.Constants
{
    public class UserUriConstants
    {
        private const string Controller = "users";
        public const string GetAll = $"{Controller}";
        public const string GetById = $"{Controller}/{{id}}";
        public const string GetBynationalCode = $"{Controller}/nationalCode";
        public const string GetByRoleAndUserName = $"{Controller}/role/{{userName}}";
        public const string UserNameExists = $"{Controller}/username/exists/{{userName}}";
        public const string Create = $"{Controller}";
        public const string Update = $"{Controller}/Update/{{id}}";
        public const string Delete = $"{Controller}/Delete/{{id}}";
        public const string AddLessonForTeacher = $"{Controller}/Teacher/Lesson";
        public const string GetAllTeachers = $"{Controller}/Teacher";
        public const string GetByIdTeachers = $"{Controller}/Teacher/{{id}}";
    }
}
