using System.Text.Json.Serialization;
using Common.Contracts.Shared;

namespace Common.Contracts.Http;

public sealed class ResponseErrorContext
{
    private ResponseErrorContext(IReadOnlyList<ErrorContext>? context)
    {
        Context = context;
    }

    [JsonPropertyName("error")]
    public IReadOnlyList<ErrorContext>? Context { get; }

    public static ResponseErrorContext Empty() => new(null);

    public static ResponseErrorContext ValidatorError(params ErrorContext[] errors) => WithContext(errors);
    
    public static ResponseErrorContext NotFoundError(params ErrorContext[] errors) => WithContext(errors);
    
    public static ResponseErrorContext BusinessRuleError(params ErrorContext[] errors) => WithContext(errors);
    
    public static ResponseErrorContext SystemError(params ErrorContext[] errors) => WithContext(errors);
    
    public static ResponseErrorContext UnauthorizedError(params ErrorContext[] errors) => WithContext(errors);
    
    public static ResponseErrorContext ConflictError(params ErrorContext[] errors) => WithContext(errors);

    public static ResponseErrorContext WithContext(params ErrorContext[] errors)
    {
        if (errors == null || errors.Length == 0)
        {
            throw new ArgumentException("At least one error context must be provided.", nameof(errors));
        }

        return new ResponseErrorContext(errors);
    }

    public static ResponseErrorContext WithContext(IEnumerable<ErrorContext> errors)
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
