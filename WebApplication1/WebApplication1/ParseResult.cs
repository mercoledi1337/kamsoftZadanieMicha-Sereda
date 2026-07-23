namespace WebApplication1;

public record ParseResult(

    bool Success,
    int ProcessedCount,
    List<Dictionary<string, object?>>? Data = null,
    string? Error = null
);