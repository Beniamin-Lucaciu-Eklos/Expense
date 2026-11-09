using System;
using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Domain.Data.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.TransactionQueries;

public sealed record GetTransactionByIdQuery(int id) : IQuery<TransactionResponse>;

internal sealed class GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository) : IQueryHandler<GetTransactionByIdQuery, TransactionResponse>
{
    public async Task<Result<TransactionResponse>> Handle(GetTransactionByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await transactionRepository.GetByIdAsync(query.id);
        if (result is null)
            return Result.Failure<TransactionResponse>(TransactionErrors.NotFound);

        var response = result.AsResponse();
        return response;
    }
}
