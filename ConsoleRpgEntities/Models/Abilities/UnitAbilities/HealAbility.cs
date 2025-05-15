using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Units.Abstracts;

namespace ConsoleRpgEntities.Models.Abilities.UnitAbilities;

public class HealAbility : Ability
{
    // HealAbility represents the ability to heal.
    public override string AbilityType { get; set; } = "HealAbility";

    public HealAbility() : base()
    {
        Name = "Heal";
        Description = "Heals a friendly unit.";
    }
    public override void Execute(Unit unit, Unit target)
    {
        if(CanUseAbility(unit))
        {
            Encounter encounter = new(unit, target);
            if (encounter.IsCrit())
            {
                Console.WriteLine($"{unit.Name} critically heals {target.Name} for {encounter.Damage + 6} hit points!");
                target.Damage((encounter.Damage + 6) * -1);
            }
            else if (encounter.IsHit())
            {
                Console.WriteLine($"{unit.Name} heals {target.Name} for {encounter.Damage + 6} hit points.");
                target.Damage((encounter.Damage + 6) * -1);
            }
            else
            {
                Console.WriteLine($"{unit.Name}'s misses {target.Name}");
            }
        }
        else
        {
            Console.WriteLine($"{unit} does not have the ability to heal.");
        }
    }
}
