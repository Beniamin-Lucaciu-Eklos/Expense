using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.TransactionQueries;

public sealed record GetMonthlyTransactionSummaryQuery(int Year, int Month)
    : IQuery<MonthlyTransactionSummaryResponse>;

internal sealed class GetMonthlyTransactionSummaryQueryHandler(
    ITransactionRepository transactionRepository,
    IUserService userService)
    : IQueryHandler<GetMonthlyTransactionSummaryQuery, MonthlyTransactionSummaryResponse>
{
    public async Task<Result<MonthlyTransactionSummaryResponse>> Handle(
        GetMonthlyTransactionSummaryQuery query,
        CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdFromNameIdentifierClaim();
        if (userId.IsFailure)
        {
            return Result.Failure<MonthlyTransactionSummaryResponse>(
                TransactionUnAuthorizedError.Create());
        }

        var startDate = new DateTime(query.Year, query.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = startDate.AddMonths(1);
        var totals = await transactionRepository.GetMonthlyTotalsAsync(
            userId.Value,
            startDate,
            endDate,
            cancellationToken);

        return Result.Success(new MonthlyTransactionSummaryResponse(
            query.Year,
            query.Month,
            totals.Income,
            totals.Expenses,
            totals.Income - totals.Expenses,
            totals.TransactionCount));
    }
}
