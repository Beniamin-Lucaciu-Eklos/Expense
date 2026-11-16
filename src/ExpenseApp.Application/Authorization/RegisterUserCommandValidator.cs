using FluentValidation;

namespace ExpenseApp.Application.Authorization;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.RegisterUserDto.Password).NotEmpty();

        RuleFor(u => u.RegisterUserDto.Email).NotEmpty()
                                             .MaximumLength(330);
    }
}
