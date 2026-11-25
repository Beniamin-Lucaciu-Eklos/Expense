namespace ExpenseApp.Application.Transactions.TransactionQueries;

public sealed record MonthlyTransactionSummaryResponse(
    int Year,
    int Month,
    double TotalIncome,
    double TotalExpenses,
    double NetBalance,
    int TransactionCount);
