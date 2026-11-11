using System;
using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.CreateTransaction;

public sealed record CreateTransactionCommand(
    TransactionType Type,
    double Amount,
    string Category,
    string Description) : ICommand<int>;
