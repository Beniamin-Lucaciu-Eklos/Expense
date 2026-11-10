using System;
using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.TransactionQueries;

public sealed record GetAllTransactionsQuery() : IQuery<IReadOnlyList<TransactionResponse>>;

internal sealed class GetAllTransactionsQueryHandler(ITransactionRepository transactionRepository)
     : IQueryHandler<GetAllTransactionsQuery, IReadOnlyList<TransactionResponse>>
{
    public async Task<Result<IReadOnlyList<TransactionResponse>>> Handle(GetAllTransactionsQuery query, CancellationToken cancellationToken)
    {
        var results = await transactionRepository.GetAllAsync();

        var response = results.AsResponse();

        return Result.Success(response);
    }
}
