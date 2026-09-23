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
                .WithMessage(StudentValidationMessages.RequiredStudentId);// تعداد رقمش رو هم درست کن
            // کد دانش آموزی هم نباید از قبل وجود داشته  باشه 
            // باید چک شود
            RuleFor(x => x.Field_Of_Study)
                .IsInEnum();
            RuleFor(x => x.Grade_Level)
                .IsInEnum();

        }
    }
}
