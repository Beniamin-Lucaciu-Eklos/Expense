using System;
using ExpenseApp.Domain.Abstractions;

namespace ExpenseApp.Domain.Data.Models;

public static class TransactionErrors
{
  public static Error NotFound = new("Transaction.NotFound",
"The transaction with the specified identifier was not found");

}
