using System;
using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.DeleteTransaction;

public sealed record DeleteTransactionCommand(int id) : ICommand<bool>;

public sealed class DeleteTransactionHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IUserService userService) : ICommandHandler<DeleteTransactionCommand, bool>
{
    public async Task<Result<bool>> Handle(DeleteTransactionCommand command, CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdFromNameIdentifiedClaim();
        if (userId.IsFailure)
            return Result.Failure<bool>(TransactionUnAuthorizedError.Create());

        var result = await transactionRepository.Delete(command.id, userId.Value);

        await unitOfWork.SaveChangesAsync();

        return result is not null ? true : Result.Failure<bool>(TransactionNotFoundError.Create());
    }
}
