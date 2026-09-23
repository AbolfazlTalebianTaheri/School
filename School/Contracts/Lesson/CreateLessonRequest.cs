using School.Domain.Enums;

namespace School.Api.Contracts.Lesson
{
    public record CreateLessonRequest(string LessonName, Grade_Level Grade_Level, Field_Of_Study Field_Of_Study);
}
