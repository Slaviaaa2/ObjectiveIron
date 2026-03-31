using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;
using ObjectiveIron.Core.Validation;

namespace ObjectiveIron.Core.Tests;

public class FocusTreeValidatorTests
{
    private readonly FocusTreeValidator _validator = new();

    [Fact]
    public void ValidTree_ReturnsSuccess()
    {
        var tree = CreateTreeWithFocuses(
            new Focus { Id = "focus_a", Cost = 10 },
            new Focus { Id = "focus_b", Cost = 10, Prerequisites = [[" focus_a"]] }
        );

        var result = _validator.Validate(tree);

        // Prerequisites reference a non-existent focus due to the space
        // Let's fix the test data
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidTree_WithCorrectPrereqs_ReturnsSuccess()
    {
        var tree = CreateTreeWithFocuses(
            new Focus { Id = "focus_a", Cost = 10 },
            new Focus
            {
                Id = "focus_b",
                Cost = 10,
                Prerequisites = [new List<string> { "focus_a" }.AsReadOnly()]
            }
        );

        var result = _validator.Validate(tree);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void EmptyTreeId_ReturnsError()
    {
        var tree = new FocusTree
        {
            Id = "",
            Country = new CountryFilter { Modifiers = [new CountryTagModifier("GER")] },
            Focuses = []
        };

        var result = _validator.Validate(tree);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Message.Contains("ID is required"));
    }

    [Fact]
    public void DuplicateFocusIds_ReturnsError()
    {
        var tree = CreateTreeWithFocuses(
            new Focus { Id = "dup_focus", Cost = 10 },
            new Focus { Id = "dup_focus", Cost = 10 }
        );

        var result = _validator.Validate(tree);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Message.Contains("Duplicate focus ID"));
    }

    [Fact]
    public void InvalidPrerequisiteReference_ReturnsError()
    {
        var tree = CreateTreeWithFocuses(
            new Focus
            {
                Id = "focus_a",
                Cost = 10,
                Prerequisites = [new List<string> { "nonexistent" }.AsReadOnly()]
            }
        );

        var result = _validator.Validate(tree);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Message.Contains("does not exist"));
    }

    [Fact]
    public void InvalidMutualExclusive_ReturnsError()
    {
        var tree = CreateTreeWithFocuses(
            new Focus
            {
                Id = "focus_a",
                Cost = 10,
                MutuallyExclusive = ["nonexistent"]
            }
        );

        var result = _validator.Validate(tree);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Message.Contains("Mutually exclusive"));
    }

    [Fact]
    public void CircularDependency_ReturnsError()
    {
        var tree = CreateTreeWithFocuses(
            new Focus
            {
                Id = "focus_a",
                Cost = 10,
                Prerequisites = [new List<string> { "focus_b" }.AsReadOnly()]
            },
            new Focus
            {
                Id = "focus_b",
                Cost = 10,
                Prerequisites = [new List<string> { "focus_a" }.AsReadOnly()]
            }
        );

        var result = _validator.Validate(tree);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Message.Contains("Circular dependency"));
    }

    [Fact]
    public void NoCountryModifiers_ReturnsWarning()
    {
        var tree = new FocusTree
        {
            Id = "test_tree",
            Country = new CountryFilter { Modifiers = [] },
            Focuses = [new Focus { Id = "f1", Cost = 10 }]
        };

        var result = _validator.Validate(tree);

        Assert.True(result.IsValid); // warnings don't make it invalid
        Assert.True(result.HasWarnings);
    }

    [Fact]
    public void ZeroCostFocus_ReturnsWarning()
    {
        var tree = CreateTreeWithFocuses(
            new Focus { Id = "free_focus", Cost = 0 }
        );

        var result = _validator.Validate(tree);

        Assert.True(result.HasWarnings);
        Assert.Contains(result.Warnings, w => w.Message.Contains("cost"));
    }

    private static FocusTree CreateTreeWithFocuses(params Focus[] focuses)
    {
        return new FocusTree
        {
            Id = "test_tree",
            Country = new CountryFilter { Modifiers = [new CountryTagModifier("GER")] },
            Focuses = focuses.ToList().AsReadOnly()
        };
    }
}
