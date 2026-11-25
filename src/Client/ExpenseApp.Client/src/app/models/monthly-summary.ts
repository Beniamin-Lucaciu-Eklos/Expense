export interface TransactionTotals {
  totalIncome: number;
  totalExpenses: number;
  netBalance: number;
  transactionCount: number;
}

export interface MonthlySummary extends TransactionTotals {
  year: number;
  month: number;
}
