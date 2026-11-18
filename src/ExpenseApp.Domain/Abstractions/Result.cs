using System;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseApp.Domain.Abstractions;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error is not NoneError)
        {
            throw new InvalidOperationException();
        }
        if (!isSuccess && error is NoneError)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, new NoneError());

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, NoneError.Create());

    public static Result<TValue> Failure<TValue>(Error error) => new(default!, false, error);

    public static Result<TValue> Create<TValue>(TValue? value)
        => value is not null ? Success(value) : Failure<TValue>(ErrorNullValue.Create());
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue value, bool isSuccess, Error error)
    : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}