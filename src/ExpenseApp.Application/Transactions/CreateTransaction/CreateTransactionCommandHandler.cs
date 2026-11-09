using System;
using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Domain.Data.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.CreateTransaction;

internal sealed class CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateTransactionCommand, int>
{
    public async Task<Result<int>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        var transaction = new Transaction
        {
            Type = command.Type,
            Amount = command.Amount,
            Category = command.Category,
            Description = command.Description,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow,
        };

        await transactionRepository.CreateAsync(transaction);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}
