using System.Collections.Immutable;

namespace ObjectiveIron.Core.Validation;

/// <summary>
/// Result of a validation operation containing errors and warnings.
/// </summary>
public sealed record class ValidationResult
{
    public ImmutableList<ValidationMessage> Errors { get; init; } = ImmutableList<ValidationMessage>.Empty;
    public ImmutableList<ValidationMessage> Warnings { get; init; } = ImmutableList<ValidationMessage>.Empty;

    public bool IsValid => Errors.Count == 0;
    public bool HasWarnings => Warnings.Count > 0;

    public static ValidationResult Success() => new();

    public static ValidationResult WithError(string message, string? context = null) =>
        new() { Errors = ImmutableList.Create(new ValidationMessage(message, context)) };

    public ValidationResult AddError(string message, string? context = null) =>
        this with { Errors = Errors.Add(new ValidationMessage(message, context)) };

    public ValidationResult AddWarning(string message, string? context = null) =>
        this with { Warnings = Warnings.Add(new ValidationMessage(message, context)) };

    public ValidationResult Merge(ValidationResult other) =>
        new()
        {
            Errors = Errors.AddRange(other.Errors),
            Warnings = Warnings.AddRange(other.Warnings)
        };

    public override string ToString()
    {
        var parts = new List<string>();
        if (Errors.Count > 0)
            parts.Add($"{Errors.Count} error(s): {string.Join("; ", Errors)}");
        if (Warnings.Count > 0)
            parts.Add($"{Warnings.Count} warning(s): {string.Join("; ", Warnings)}");
        return parts.Count > 0 ? string.Join(" | ", parts) : "Valid";
    }
}

public sealed record ValidationMessage(string Message, string? Context = null)
{
    public override string ToString() =>
        Context is not null ? $"[{Context}] {Message}" : Message;
}
