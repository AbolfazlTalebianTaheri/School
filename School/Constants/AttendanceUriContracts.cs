namespace School.Api.Constants
{
    public static class AttendanceUriContracts
    {
        private const string Controller = "attendances";
        public const string AddDay = $"{Controller}/day";
        public const string AddDelay = $"{Controller}/delay";
        public const string AddLog = $"{Controller}/log";
        public const string DayExists = $"{Controller}/{{date}}";
        public const string DelayExists = $"{Controller}";
        public const string DeleteDay = $"{Controller}/day";
        public const string DeleteDelay = $"{Controller}/delay";
        public const string DeleteLog = $"{Controller}/log";
        public const string GetAbsentsByDate = $"{Controller}/{{date}}/absents";
        public const string GetDayByDate = $"{Controller}/{{date}}/day";
        public const string GetDayById = $"{Controller}/{{id}}/day";
        public const string GetDelayById = $"{Controller}/{{id}}/delay";
        public const string GetDelaysByDate = $"{Controller}/{{date}}/delay";
        public const string GetLogById = $"{Controller}/{{id}}/log";
        public const string GetStudentDelays = $"{Controller}/delay";
        public const string GetStudentLog = $"{Controller}/log";
        public const string StudentLogExists = $"{Controller}/log/exists";
        public const string UpdateDay = $"{Controller}/day";
        public const string UpdateDelay = $"{Controller}/delay";
        public const string UpdateLog = $"{Controller}/log";
    }
}
