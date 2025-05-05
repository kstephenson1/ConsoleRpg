using ConsoleRpgEntities.Models.Units.Abstracts;

namespace ConsoleRpgEntities.Models.Abilities;

public class FlyAbility : Ability
{
    // FlyAbility represents the ability to fly.
    public override string AbilityType { get; set; } = "FlyAbility";

    public FlyAbility()
    {
        Name = "Fly";
        Description = "Flies to a location within flight range.";
    }

    public override void Execute(Unit unit, Unit target)
    {
        if(CanUseAbility(unit))
        {
            Console.WriteLine($"{unit.Name} flies.");
        }
        else
        {
            Console.WriteLine($"{unit.Name} does not have the ability to fly.");
        }
    }
}
