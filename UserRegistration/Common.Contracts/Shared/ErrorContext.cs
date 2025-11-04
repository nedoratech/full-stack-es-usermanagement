namespace Common.Contracts.Shared;

public class ErrorContext(ErrorType errorType)
{
    private readonly Dictionary<string, string> _context = new();

    protected internal ErrorType ErrorType { get; } = errorType;
    protected internal IReadOnlyDictionary<string, string> Context => _context;

    public ErrorContext AddContext(string key, string code)
    {
        _context[key] = code;
        return this;
    }
}

