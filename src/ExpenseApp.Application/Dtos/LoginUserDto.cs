using System;

namespace ExpenseApp.Application.Dtos;

public sealed record LoginUserDto(string Email, string Password);
