using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Items.EquippableItems.WeaponItems;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;
using ConsoleRpgEntities.Models.Interfaces.UnitBehaviors;

namespace ConsoleRpgEntities.Models.Commands.UnitCommands;

public class ShootCommand : ICommand
{
    // ShootCommand is used to shoot a target from a unit.  It takes in a unit and a target, and shoots the target
    // from the unit. It also prints a message to the console indicating that the target has been shot.


    private readonly IUnit _unit;
    private readonly IUnit _target;
    private readonly Encounter _encounter;
    public ShootCommand() { }
    public ShootCommand(IUnit unit, IUnit target)
    {
        _unit = unit;
        _target = target;
        _encounter = new(unit, target);
    }

    public void Execute()
    {
        if (_unit is IShootable)
        {
            if (_unit != _target)
            {
                if (_encounter.Unit.GetEquippedWeapon() is PhysicalWeaponItem)
                {
                    Console.WriteLine($"{_unit.Name} attacks {_target.Name} with {_encounter.Unit.GetEquippedWeapon().Name}\n");
                    Console.WriteLine($"Hit Chance: {_encounter.GetDisplayedHit()}");
                    Console.WriteLine($"Critical Strike Chance: {_encounter.GetDisplayedCrit()}");
                    Console.WriteLine($"{_unit.Name}'s Damage: {_encounter.GetAttack()}");
                    Console.WriteLine($"{_target.Name}'s Defense: {_encounter.GetTargetPhysicalResiliance()}");
                }
                else if (_encounter.Unit.GetEquippedWeapon() is MagicWeaponItem)
                {
                    Console.WriteLine($"{_unit.Name} casts {_encounter.Unit.GetEquippedWeapon().Name} at {_target.Name}\n");
                    Console.WriteLine($"Hit Chance: {_encounter.GetDisplayedHit()}");
                    Console.WriteLine($"Critical Strike Chance: {_encounter.GetDisplayedCrit()}");
                    Console.WriteLine($"{_unit.Name}'s Magic Damage: {_encounter.GetMagicAttack()}");
                    Console.WriteLine($"{_target.Name}'s Resistance: {_encounter.GetTargetMagicResiliance()}\n");
                }

                Console.WriteLine($"{_unit.Name} rolls a : {_encounter.Roll}");

                if (_encounter.IsCrit())
                {
                    Console.WriteLine($"{_unit.Name} critically hit {_target.Name} for {_encounter.Damage} damage!");
                    _target.Damage(_encounter.Damage);
                }
                else if (_encounter.IsHit())
                {
                    Console.WriteLine($"{_unit.Name} hit {_target.Name} for {_encounter.Damage} damage.");
                    _target.Damage(_encounter.Damage);
                }
                else
                {
                    Console.WriteLine($"{_unit.Name}'s misses {_target.Name}");
                }
            }
            else
            {
                Console.WriteLine($"{_unit.Name} should not attack themselves.  That's not very nice!");
            }
        }
        else
        {
            Console.WriteLine($"{_unit} cannot attack.");
        }
    }
}
