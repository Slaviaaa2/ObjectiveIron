namespace ObjectiveIron.Core.Models.Primitives;

/// <summary>
/// Clausewitz comparison operators used in triggers and conditions.
/// </summary>
public enum Operator
{
    /// <summary>Equals (=)</summary>
    Equals,

    /// <summary>Less than (&lt;)</summary>
    LessThan,

    /// <summary>Greater than (&gt;)</summary>
    GreaterThan,

    /// <summary>Greater than or equal (&gt;=)</summary>
    GreaterThanOrEqual,

    /// <summary>Less than or equal (&lt;=)</summary>
    LessThanOrEqual,

    /// <summary>Not equals (!=) — rarely used in Clausewitz but supported</summary>
    NotEquals
}

public static class OperatorExtensions
{
    public static string ToClausewitz(this Operator op) => op switch
    {
        Operator.Equals => "=",
        Operator.LessThan => "<",
        Operator.GreaterThan => ">",
        Operator.GreaterThanOrEqual => ">=",
        Operator.LessThanOrEqual => "<=",
        Operator.NotEquals => "!=",
        _ => "="
    };
}
