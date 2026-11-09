using System;

namespace ExpenseApp.Application.Dtos;

public record TransactionDto(
    string Type,
    double Amount,
    string Category,
    string Description);
