namespace School.Domain.Exceptions
{
    public class DuplicateStudentException(string message) : Exception(message)
    {
    }
}
