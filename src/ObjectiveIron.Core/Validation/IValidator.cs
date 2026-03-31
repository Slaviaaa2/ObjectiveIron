namespace ObjectiveIron.Core.Validation;

/// <summary>
/// Validation interface for domain models.
/// </summary>
public interface IValidator<in T>
{
    ValidationResult Validate(T model);
}
