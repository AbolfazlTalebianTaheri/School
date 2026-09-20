namespace School.Api.Constants
{
    public static class AttendanceUriContracts
    {
        private const string Controller = "attendances";
        public const string AddDay = $"{Controller}";
        public const string AddDelay = $"{Controller}";
        public const string AddLog = $"{Controller}";
        public const string DayExists = $"{Controller}/{{date}}";
        public const string DelayExists = $"{Controller}";
        public const string DeleteDay = $"{Controller}";
        public const string DeleteDelay = $"{Controller}";
        public const string DeleteLog = $"{Controller}";
        public const string GetAbsentsByDate = $"{Controller}/{{date}}";
        public const string GetDayByDate = $"{Controller}/{{date}}";
        public const string GetDayById = $"{Controller}/{{id}}";
        public const string GetDelayById = $"{Controller}/{{id}}";
        public const string GetDelaysByDate = $"{Controller}/{{date}}";
        public const string GetLogById = $"{Controller}/{{id}}";
        public const string GetStudentDelays = $"{Controller}";
        public const string GetStudentLog = $"{Controller}";
        public const string StudentLogExists = $"{Controller}";
        public const string UpdateDay = $"{Controller}";
        public const string UpdateDelay = $"{Controller}";
        public const string UpdateLog = $"{Controller}";
    }
}
