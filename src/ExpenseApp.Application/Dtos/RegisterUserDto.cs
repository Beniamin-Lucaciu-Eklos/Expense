using System;

namespace ExpenseApp.Application.Dtos;

public sealed record RegisterUserDto(
    string Email,
    string Password);
