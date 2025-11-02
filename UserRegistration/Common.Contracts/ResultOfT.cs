using Common.Contracts.Http;
using Common.Contracts.Shared;

namespace Common.Contracts;

public sealed class Result<T> : Result
{
    private readonly T? _value;

    private Result(T value)
    {
        _value = value;
    }

    private Result(ErrorContext error) : base(error)
    {
    }

    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("Cannot access Value when result is a failure.");
            }

            return _value!;
        }
    }

    public static Result<T> Success(T value) => new(value);
    
    public new static Result<T> Failure(ErrorContext error) => new(error);
    
    public static implicit operator Result<T>(T value) => Success(value);
    
    public static implicit operator Result<T>(ErrorContext error) => Failure(error);
}
