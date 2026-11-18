namespace ExpenseApp.Domain.Abstractions;

public enum ErrorType
{
    None,
    NullValue,
    Failure,
    Validation,
    NotFound,
    Conflict,
    Unauthorized
}
