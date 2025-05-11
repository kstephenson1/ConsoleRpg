using ConsoleRpgEntities.Models.Units.Abstracts;

namespace ConsoleRpgEntities.Models.Abilities.UnitAbilities;

public class StealAbility : Ability
{
    // StealAbility represents the ability to steal an item from an enemy.
    public override string AbilityType { get; set; } = "StealAbility";

    public StealAbility() : base()
    {
        Name = "Steal";
        Description = "Steals an item from an enemy.";
    }


    public override void Execute(Unit unit, Unit target)
    {
        if(CanUseAbility(unit))
        {
            Console.WriteLine($"{unit.Name} steals an item from {target.Name}.");
        }
        else
        {
            Console.WriteLine($"{unit.Name} does not have the ability to steal.");
        }
    }
}
