using System;
using FluentValidation;

namespace ExpenseApp.Application.Transactions.CreateTransaction;

internal sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(c => c.Type).NotEmpty();

        RuleFor(c => c.Category).NotEmpty();

        RuleFor(c => c.Amount).NotEqual(0);
    }
}