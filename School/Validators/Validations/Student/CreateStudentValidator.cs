using FluentValidation;
using School.Api.Contracts.Student;
using School.Api.Validators.Messages;

namespace School.Api.Validators.Validations.Student
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentRequest>
    {
        public CreateStudentValidator()
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
