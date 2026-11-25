namespace ExpenseApp.Application.Transactions.TransactionQueries;

public sealed record PagedTransactionsResponse(
    IReadOnlyList<TransactionResponse> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);
