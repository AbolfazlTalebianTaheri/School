using FluentValidation;
using School.Api.Contracts.Student;
using School.Api.Validators.Messages;
using System.Security.Cryptography.Xml;

namespace School.Api.Validators.Validations.Student
{
    public class UpdateStudentValidator : AbstractValidator<StudentResponse>
    {
        public UpdateStudentValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage(StudentValidationMessages.RequiredFirstName)
                .Length(3, 20)
                .WithMessage(StudentValidationMessages.InvalidLength);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage(StudentValidationMessages.RequiredFirstName)
                .Length(3, 20)
                .WithMessage(StudentValidationMessages.InvalidLength);
            RuleFor(x => x.StudentCode)
                .NotEmpty()
                .WithMessage(StudentValidationMessages.RequiredStudentId)
                .InclusiveBetween(100000, 999999)
                .WithMessage("کد دانش‌آموزی باید دقیقاً ۶ رقم باشد.");
            RuleFor(x => x.NationalCode)
                .NotEmpty()
                .WithMessage("کد ملی اجباری است.")
                .Matches(@"^\d{10}$")
                .WithMessage("کد ملی باید دقیقاً ۱۰ رقم باشد.");
            RuleFor(x => x.Field_Of_Study)
                .IsInEnum();
            RuleFor(x => x.Grade_Level)
                .IsInEnum();

        }
    }
}
