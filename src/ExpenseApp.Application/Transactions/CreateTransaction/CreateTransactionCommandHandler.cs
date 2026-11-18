using System;
using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.CreateTransaction;

internal sealed class CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IUserService userService) : ICommandHandler<CreateTransactionCommand, int>
{
    public async Task<Result<int>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdFromNameIdentifiedClaim();
        if(userId.IsFailure)
           return Result.Failure<int>(TransactionUnAuthorizedError.Create());       

        var transaction = Transaction.Create(
            command.Type,
            command.Amount,
            command.Category,
            command.Description,
            userId.Value);

        await transactionRepository.CreateAsync(transaction);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}
