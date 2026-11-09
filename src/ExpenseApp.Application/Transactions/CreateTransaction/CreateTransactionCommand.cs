using System;
using ExpenseApp.Application.Abstractions.Messaging;

namespace ExpenseApp.Application.Transactions.CreateTransaction;

public sealed record CreateTransactionCommand(
    string Type,
    double Amount,
    string Category,
    string Description) : ICommand<int>;
