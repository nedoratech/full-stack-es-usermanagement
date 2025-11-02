using Common.Contracts.Shared;

namespace Common.Contracts;

public class Result
{
    protected Result(ErrorContext? error = null)
    {
        Error = error;
    }

    public bool IsSuccess => Error == null;
    public bool IsFailure => !IsSuccess;
    public ErrorContext? Error { get; }

    public static Result Success() => new();
    
    public static Result Failure(ErrorContext error) => new(error);
}

