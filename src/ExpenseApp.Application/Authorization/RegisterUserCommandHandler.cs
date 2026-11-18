using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Application.Dtos;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ExpenseApp.Application.Authorization;

public sealed record RegisterUserCommand(RegisterUserDto RegisterUserDto) : ICommand<RegisterUserResponse>;

public sealed record RegisterUserResponse(string Token, string Email);

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.RegisterUserDto.Password).NotEmpty();

        RuleFor(u => u.RegisterUserDto.Email).NotEmpty()
                                             .MaximumLength(330);
    }
}

internal class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher<User> userPasswordHasher,
    IUnitOfWork uow) : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var registerDto = command.RegisterUserDto;
        if (await userRepository.ExistsAsync(registerDto.Email))
            return Result.Failure<RegisterUserResponse>(UserEmailAlreadyExistsError.Create());

        var passwordHasshed = userPasswordHasher.HashPassword(null!, registerDto.Password);

        var user = User.Create(registerDto.Email, passwordHasshed);
        await userRepository.AddAsync(user);
        await uow.SaveChangesAsync();

        return Result.Success(new RegisterUserResponse(user.Token, user.Email));
    }
}