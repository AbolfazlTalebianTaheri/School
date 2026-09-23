using FluentValidation;
using School.Api.Contracts.Lesson;
using School.Api.Validators.Messages;

namespace School.Api.Validators.Validations.Lesson
{
    public class UpdateLessonValidator : AbstractValidator<UpdateLessonRequest>
    {
        public UpdateLessonValidator()
        {
            RuleFor(x => x.LessonName)
                .NotEmpty()
                .WithMessage(LessonValidationMessages.RequiredLessonName);
            RuleFor(x => x.Field_Of_Study)
                .NotEmpty()
                .WithMessage(LessonValidationMessages.RequiredField_Of_Study)
                .IsInEnum();
            RuleFor(x => x.Grade_Level)
                .NotEmpty()
                .WithMessage(LessonValidationMessages.RequiredGrade_Level)
                .IsInEnum();
        }
    }
}
