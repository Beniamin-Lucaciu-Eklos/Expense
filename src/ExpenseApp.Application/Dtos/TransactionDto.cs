using System;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Dtos;

public record TransactionDto(
    TransactionType Type,
    double Amount,
    string Category,
    string Description);
