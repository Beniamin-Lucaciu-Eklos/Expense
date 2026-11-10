using System;
using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Application.Dtos;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.UpdateTransaction;

public sealed record UpdateTransactionCommand(int Id, TransactionDto Dto) : ICommand<int>;
internal sealed class UpdateTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateTransactionCommand, int>
{
    public async Task<Result<int>> Handle(UpdateTransactionCommand command, CancellationToken cancellationToken)
    {      
        var transaction = Transaction.Create(command.Dto.Type, command.Dto.Amount, command.Dto.Category, command.Dto.Description);
        var transactionFromDb = await transactionRepository.UpdateAsync(command.Id, transaction);
        if(transactionFromDb is null)
           return Result.Failure<int>(TransactionErrors.NotFound);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return transactionFromDb.Id;
    }
}