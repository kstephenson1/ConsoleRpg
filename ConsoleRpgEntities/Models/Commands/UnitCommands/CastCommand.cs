using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;
using ConsoleRpgEntities.Models.Interfaces.UnitBehaviors;

namespace ConsoleRpgEntities.Models.Commands.UnitCommands;

public class CastCommand : ICommand
{
    // CastCommand is used to cast a spell from a unit.  It takes in a unit and a spell name, and casts the spell
    // from the unit. It also prints a message to the console indicating that the spell has been cast. If the unit is
    // not a spellcaster, a message is printed to the console indicating that the unit is not a spellcaster.

    private readonly IUnit _unit;
    private readonly string _spellName;
    public CastCommand() { }
    public CastCommand(IUnit unit, string spellName)
    {
        _unit = unit;
        _spellName = spellName;
    }
    public void Execute()
    {
        if (_unit is ICastable)
        {
            Console.WriteLine($"{_unit.Name} casts {_spellName}");
        }
        else
        {
            Console.WriteLine($"{_unit.Name} is not a spellcaster!");
        }
    }
}
