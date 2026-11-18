using System;
using ExpenseApp.Domain.Abstractions;

namespace ExpenseApp.Domain.Data.Models;

public sealed record TransactionNotFoundError()
  : Error("TransactionNotFound", "The transaction with the specified identifier was not found", ErrorType.NotFound)
{
  public static TransactionNotFoundError Create() => new TransactionNotFoundError();
}

public sealed record TransactionUnAuthorizedError()
  : Error("TransactionUnAuthorized",
      "The transaction action iss UnAuthorized",
      ErrorType.Unauthorized)
{
  public static TransactionUnAuthorizedError Create() => new TransactionUnAuthorizedError();
}

