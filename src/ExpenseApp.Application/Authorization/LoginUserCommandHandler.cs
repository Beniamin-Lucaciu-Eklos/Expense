using System;
using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Application.Dtos;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ExpenseApp.Application.Authorization;

public sealed record LoginUserCommand(LoginUserDto UserDto) : ICommand<LoginUserResponse>;

public sealed record LoginUserResponse(string Token, string Email);

internal class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(u => u.UserDto.Email).NotEmpty();

        RuleFor(u => u.UserDto.Password).NotEmpty();
    }
}

internal sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher<User> userPasswordHasher,
    IUserService userService) : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        LoginUserDto userDto = command.UserDto;
        if (await userRepository.ExistsAsync(userDto.Email, userDto.Password, cancellationToken))
            return Result.Failure<LoginUserResponse>(UserErrors.InvalidCredentials);

        var user = await userRepository.Load(userDto.Email, cancellationToken);
        if (user is null)
            return Result.Failure<LoginUserResponse>(UserErrors.NotFound);

        var result = userPasswordHasher.VerifyHashedPassword(user, user.Password, userDto.Password);
        if (result == PasswordVerificationResult.Failed)
            return Result.Failure<LoginUserResponse>(UserErrors.InvalidCredentials);

        var token = userService.GenerateJwtToken(user);
        return Result.Success(new LoginUserResponse(token, user.Email));
    }
}
