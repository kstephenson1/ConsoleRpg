using ConsoleRpgEntities.Models.Units.Abstracts;

namespace ConsoleRpgEntities.Models.Abilities.UnitAbilities;

public class FlyAbility : Ability
{
    // FlyAbility represents the ability to fly.
    public override string AbilityType { get; set; } = "FlyAbility";

    public FlyAbility() : base()
    {
        Name = "Fly";
        Description = "Flies to a target enemy within flight range.";
    }

    public override void Execute(Unit unit, Unit target)
    {
        if(CanUseAbility(unit))
        {
            Console.WriteLine($"{unit.Name} flies to {target.Name}.");
        }
        else
        {
            Console.WriteLine($"{unit.Name} does not have the ability to fly.");
        }
    }
}
