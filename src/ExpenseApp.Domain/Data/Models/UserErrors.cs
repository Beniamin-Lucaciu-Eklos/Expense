using System;
using ExpenseApp.Domain.Abstractions;

namespace ExpenseApp.Domain.Data.Models;

public sealed record UserNotFoundError() : Error("UserNotFound",
"The user not found", ErrorType.NotFound)
{
  public static UserNotFoundError Create() => new UserNotFoundError();
}

public sealed record UserEmailAlreadyExistsError() 
: Error("UserEmailAlreadyExists",
"The user with email already exists", ErrorType.Conflict)
{
  public static UserEmailAlreadyExistsError Create() => new UserEmailAlreadyExistsError();
}

public sealed record UserInvalidCredentialsError() 
: Error("UserInvalidCredentialsError",
"This user has invalid credentials", ErrorType.Unauthorized)
{
  public static UserInvalidCredentialsError Create() => new UserInvalidCredentialsError();
}
