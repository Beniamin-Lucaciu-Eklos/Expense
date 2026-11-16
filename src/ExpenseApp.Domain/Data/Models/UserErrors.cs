using System;
using ExpenseApp.Domain.Abstractions;

namespace ExpenseApp.Domain.Data.Models;

public static class UserErrors
{
  public static class Codes
  {
    public const string NotFound = "UserErrors.NotFound";
    public const string EmailAlreadyExists = "UserErrors.EmailAlreadyExists";
    public const string InvalidCredentials = "UserErrors.InvalidCredentials";
  }

  public static Error NotFound = new(Codes.NotFound,
"The user not found");

  public static Error EmailAlreadyExists = new(Codes.EmailAlreadyExists,
"The user with email already exists");

  public static Error InvalidCredentials = new(Codes.InvalidCredentials,
"This user has invalid credentials");
}
