using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.TransactionQueries;

public sealed record GetTransactionTotalsQuery : IQuery<TransactionTotalsResponse>;

internal sealed class GetTransactionTotalsQueryHandler(
    ITransactionRepository transactionRepository,
    IUserService userService)
    : IQueryHandler<GetTransactionTotalsQuery, TransactionTotalsResponse>
{
    public async Task<Result<TransactionTotalsResponse>> Handle(
        GetTransactionTotalsQuery query,
        CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdFromNameIdentifierClaim();
        if (userId.IsFailure)
        {
            return Result.Failure<TransactionTotalsResponse>(
                TransactionUnAuthorizedError.Create());
        }

        var totals = await transactionRepository.GetTotalsAsync(userId.Value, cancellationToken);
        return Result.Success(new TransactionTotalsResponse(
            totals.Income,
            totals.Expenses,
            totals.Income - totals.Expenses,
            totals.TransactionCount));
    }
}
