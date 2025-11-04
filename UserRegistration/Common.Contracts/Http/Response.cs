using System.Text.Json.Serialization;
using Common.Contracts.Shared;

namespace Common.Contracts.Http;

public class Response
{
    protected Response(ResponseErrorContext? error = null)
    {
        Error = error;
    }

    [JsonPropertyName("success")]
    public bool Success => Error is null;

    [JsonPropertyName("error")] 
    protected ResponseErrorContext? Error { get; private set; }

    public static Response ForSuccess() => new();

    public static Response WithError(ErrorContext error)
    {
        if (error == null)
        {
            throw new ArgumentNullException(nameof(error));
        }

        return new Response(ResponseErrorContext.WithContext(error));
    }

    public static Response WithError(params ErrorContext[] errors)
    {
        if (errors == null || errors.Length == 0)
        {
            throw new ArgumentException("At least one error context must be provided.", nameof(errors));
        }

        return new Response(ResponseErrorContext.WithContext(errors));
    }

    public static Response WithError(IEnumerable<ErrorContext> errors)
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

        return new Response(ResponseErrorContext.WithContext(errorList));
    }
}

