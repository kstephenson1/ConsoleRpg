using ConsoleRpgEntities.Models.Units.Abstracts;

namespace ConsoleRpgEntities.Models.Abilities.UnitAbilities;

public class TauntAbility : Ability
{
    // TauntAbility represents the ability to taunt an enemy to attack the unit.
    public override string AbilityType { get; set; } = "TauntAbility";

    public TauntAbility()
    {
        Name = "Taunt";
        Description = "Urges an enemy to attack you.";
    }

    public override void Execute(Unit unit, Unit target)
    {
        if (CanUseAbility(unit))
        {
            Console.WriteLine($"{unit.Name} taunts {target.Name}.");
        }
        else
        {
            Console.WriteLine($"{unit.Name} does not have the ability to taunt.");
        }
    }
}
