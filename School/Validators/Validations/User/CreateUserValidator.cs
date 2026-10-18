using FluentValidation;
using School.Api.Contracts.User;
using School.Api.Validators.Messages;

namespace School.Api.Validators.Validations.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.UserRole)
                .IsInEnum()
                .WithMessage(UserValidationMessages.StudentIdRequiredForParent);

            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber)
                    .Matches(@"^09\d{9}$")
                    .WithMessage(UserValidationMessages.PhoneNumberInvalid);
                RuleFor(x => x.StudentId)
                    .NotEmpty()
                    .When(x => x.UserRole == Domain.Enums.UserRole.Parent)
                    .WithMessage(UserValidationMessages.StudentIdRequiredForParent);
                RuleFor(x => x.UserName)
                    .Empty()
                    .WithMessage(UserValidationMessages.CantSetUserNameForParent);
                RuleFor(x => x.Password)
                    .Empty()
                    .WithMessage(UserValidationMessages.CantSetPasswordForParent);
            })
            .Otherwise(() =>
            {
                RuleFor(x => x.UserName)
                    .NotEmpty()
                    .WithMessage(UserValidationMessages.UserNameRequired)
                    .MinimumLength(3)
                    .WithMessage(UserValidationMessages.UserNameMinimumLength)
                    .MaximumLength(50)
                    .WithMessage(UserValidationMessages.UserNameMaxLength);
                RuleFor(x => x.Password)
                    .NotEmpty()
                    .WithMessage(UserValidationMessages.RequredPassword)
                    .MinimumLength(8)
                    .WithMessage(UserValidationMessages.PasswordMinimumLength)
                    .MaximumLength(50)
                    .WithMessage(UserValidationMessages.PasswordMaxLength)
                    .Matches("[A-Z]")
                    .WithMessage(UserValidationMessages.AnUppercaseLetterIsMandatory)
                    .Matches("[0-9]")
                    .WithMessage(UserValidationMessages.A_NumberIsRequired);

                RuleFor(x => x.StudentId)
                    .Empty()
                    .WithMessage(UserValidationMessages.CantSetStudentIdForAdmin);
            });
        }
    }
}
