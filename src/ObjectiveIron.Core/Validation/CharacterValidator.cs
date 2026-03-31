using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Data;

namespace ObjectiveIron.Core.Validation;

public sealed class CharacterValidator : IValidator<Character>
{
    private readonly Hoi4DataService? _dataService;

    public CharacterValidator(Hoi4DataService? dataService = null)
    {
        _dataService = dataService;
    }

    public ValidationResult Validate(Character character)
    {
        var result = ValidationResult.Success();

        if (string.IsNullOrWhiteSpace(character.Id))
            result = result.AddError("Character ID is required.", "Character");

        if (character.Roles.Count == 0)
            result = result.AddWarning($"Character '{character.Id}' has no roles.", character.Id);

        if (_dataService != null)
        {
            var unitTraits = _dataService.GetUnitTraits();
            var leaderTraits = _dataService.GetCountryLeaderTraits();

            foreach (var role in character.Roles)
            {
                if (role is CountryLeaderRole leader)
                {
                    // Ideology validation?
                    foreach (var trait in leader.Traits)
                    {
                        // Country leader traits are often different from unit leader traits
                    }
                }
                else if (role is CommanderRole commander)
                {
                    foreach (var trait in commander.Traits ?? [])
                    {
                        if (!unitTraits.Contains(trait))
                        {
                            result = result.AddWarning($"Potential invalid unit leader trait '{trait}' for character '{character.Id}'.", character.Id);
                        }
                    }
                }
            }
        }

        return result;
    }
}
