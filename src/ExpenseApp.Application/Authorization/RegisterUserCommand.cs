using System;
using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Application.Dtos;

namespace ExpenseApp.Application.Authorization;

public sealed record RegisterUserCommand(RegisterUserDto RegisterUserDto) : ICommand<RegisterUserResponse>;
