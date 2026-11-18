using System;
using ExpenseApp.Domain.Abstractions;

namespace ExpenseApp.Domain.Data.Models;

public static class TransactionErrors
{
  public static class Codes
  {
    public const string NotFound = "Transaction.NotFound";
    public const string UnAuthorized = "Transaction.UnAuthorized";
  }

  public static Error NotFound = new(Codes.NotFound,
"The transaction with the specified identifier was not found");

  public static Error UnAuthorized = new(Codes.UnAuthorized,
"The transaction action iss UnAuthorized");

}
