using System.Text.Json.Serialization;
using Common.Contracts.Shared;

namespace Common.Contracts.Http;

public sealed class Response<T> : Response
{
    private readonly T? _data;

    private Response(T? data, ResponseErrorContext? error = null) : base(error)
    {
        _data = data;
    }

    [JsonPropertyName("data")]
    public T? Data
    {
        get
        {
            if (Error is not null)
            {
                throw new InvalidOperationException("Cannot access Data when response has errors. " +
                                                    "Check Success and Error properties first.");
            }

            return _data;
        }
    }

    public new static Response<T> ForSuccess() => new(default);

    public static Response<T> WithResult(T data) => new(data);

    public new static Response<T> WithError(params ErrorContext[] errors)
    {
        if (errors == null || errors.Length == 0)
        {
            throw new ArgumentException("At least one error context must be provided.", nameof(errors));
        }

        return new Response<T>(default, ResponseErrorContext.WithContext(errors));
    }

    public new static Response<T> WithError(IEnumerable<ErrorContext> errors)
    {
        if (errors == null)
        {
            throw new ArgumentNullException(nameof(errors));
        }

        var errorList = errors.ToList();
        if (errorList.Count == 0)
        {
            throw new ArgumentException("At least one error context must be provided.", nameof(errors));
        }

        return new Response<T>(default, ResponseErrorContext.WithContext(errorList));
    }
}

