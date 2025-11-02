namespace Common.Contracts.Shared;

public abstract class ErrorContext(string errorType)
{
    private readonly Dictionary<string, string> _context = new();

    public string ErrorType { get; } = errorType;
    public IReadOnlyDictionary<string, string> Context => _context;

    public ErrorContext AddContext(string key, string code)
    {
        _context[key] = code;
        return this;
    }
}

