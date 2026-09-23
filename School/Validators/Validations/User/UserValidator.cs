using FluentValidation;
using School.Api.Contracts.User;
using School.Api.Validators.Messages;

namespace School.Api.Validators.Validations.User
{
    public class UserValidator : AbstractValidator<UserResponse>
    {
        public UserValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty()
                .When(x => x.UserRole == Domain.Enums.UserRole.Parent)
                .WithMessage(UserValidationMessages.StudentIdRequiredForParent);
            RuleFor(x => x.UserRole)
                .IsInEnum();
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .WithMessage(UserValidationMessages.PasswordMinimumLength)
                .MaximumLength(50)
                .WithMessage(UserValidationMessages.PasswordMaxLength)
                .Matches("[A-Z]")
                .WithMessage(UserValidationMessages.AnUppercaseLetterIsMandatory)
                .Matches("[0-9]")
                .WithMessage(UserValidationMessages.A_NumberIsRequired);
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage(UserValidationMessages.PhoneNumberRequired)
                .Matches(@"^09\d{9}$")
                .WithMessage(UserValidationMessages.PhoneNumberInvalid);
            RuleFor(x=>x.UserName)
                .NotEmpty()
                .WithMessage(UserValidationMessages.UserNameRequired)
                .MinimumLength(3)
                .WithMessage(UserValidationMessages.UserNameMinimumLength)
                .MaximumLength(50)
                .WithMessage(UserValidationMessages.UserNameMaxLength);
        }
    }
}
