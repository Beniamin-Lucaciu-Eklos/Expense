namespace ExpenseApp.Domain.Abstractions;

public abstract record Error(string Code, string Description, ErrorType Type);

public sealed record NoneError()
 : Error(string.Empty, string.Empty, ErrorType.None)
{
    public static NoneError Create() => new NoneError();
}

public sealed record ErrorNullValue()
 : Error("Error.NullValue", "Null value was provided", ErrorType.NullValue)
{
    public static ErrorNullValue Create() => new ErrorNullValue();
}

