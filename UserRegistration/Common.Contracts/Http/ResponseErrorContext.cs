using System.Text.Json.Serialization;
using Common.Contracts.Shared;

namespace Common.Contracts.Http;

public sealed class ResponseErrorContext
{
    private ResponseErrorContext(IReadOnlyList<ErrorContext>? context)
    {
        Context = context;
    }

    [JsonPropertyName("context")]
    public IReadOnlyList<ErrorContext>? Context { get; }

    protected internal static ResponseErrorContext WithContext(params ErrorContext[] errors)
    {
        if (errors == null || errors.Length == 0)
        {
            throw new ArgumentException("At least one error context must be provided.", nameof(errors));
        }

        return new ResponseErrorContext(errors);
    }

    protected internal static ResponseErrorContext WithContext(IEnumerable<ErrorContext> errors)
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

        return new ResponseErrorContext(errorList);
    }
}
