﻿using System.Diagnostics.CodeAnalysis;

namespace Timenote.Application.Common;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }
    
    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    
    public static Result Failure(Error error) => new(false, error);
    
    protected static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

public class Result<TValue>(TValue? value, bool isSuccess, Error error) : Result(isSuccess, error)
{
    [NotNull]
    public TValue Value => IsSuccess
        ? value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");
    
    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}