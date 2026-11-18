using System;
using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.TransactionQueries;

public sealed record GetTransactionByIdQuery(int id) : IQuery<TransactionResponse>;

internal sealed class GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository, IUserService userService) : IQueryHandler<GetTransactionByIdQuery, TransactionResponse>
{
    public async Task<Result<TransactionResponse>> Handle(GetTransactionByIdQuery query, CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdFromNameIdentifiedClaim();
        if (userId.IsFailure)
            return Result.Failure<TransactionResponse>(TransactionUnAuthorizedError.Create());

        var result = await transactionRepository.GetByIdAsync(query.id, userId.Value);
        if (result is null)
            return Result.Failure<TransactionResponse>(TransactionNotFoundError.Create());

        var response = result.AsResponse();
        return response;
    }
}
