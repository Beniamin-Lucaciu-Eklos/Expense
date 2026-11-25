using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.TransactionQueries;

public sealed record GetTransactionsPageQuery(int Page, int PageSize)
    : IQuery<PagedTransactionsResponse>;

internal sealed class GetTransactionsPageQueryHandler(
    ITransactionRepository transactionRepository,
    IUserService userService)
    : IQueryHandler<GetTransactionsPageQuery, PagedTransactionsResponse>
{
    public async Task<Result<PagedTransactionsResponse>> Handle(
        GetTransactionsPageQuery query,
        CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdFromNameIdentifierClaim();
        if (userId.IsFailure)
        {
            return Result.Failure<PagedTransactionsResponse>(
                TransactionUnAuthorizedError.Create());
        }

        var skip = (query.Page - 1) * query.PageSize;
        var result = await transactionRepository.GetPageAsync(
            userId.Value,
            skip,
            query.PageSize,
            cancellationToken);
        var response = new PagedTransactionsResponse(
            result.Items.AsResponse(),
            query.Page,
            query.PageSize,
            result.TotalCount,
            (int)Math.Ceiling(result.TotalCount / (double)query.PageSize));

        return Result.Success(response);
    }
}
