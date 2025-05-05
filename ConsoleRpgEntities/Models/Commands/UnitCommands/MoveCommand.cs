using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;

namespace ConsoleRpgEntities.Models.Commands.UnitCommands;

public class MoveCommand : ICommand
{
    // MoveCommand is used to move a unit.  It takes in a unit and moves it. It also prints a message to the console
    // indicating that the unit has moved. If the unit is not able to move, a message is printed to the console
    // indicating that the unit cannot move.

    private readonly IUnit _unit;
    public MoveCommand() { }

    public MoveCommand(IUnit unit)
    {
        _unit = unit;
    }
    public void Execute()
    {
        if (_unit is IUnit)
        {
            Console.WriteLine($"{_unit.Name} moves.");
        }
        else
        {
            Console.WriteLine($"{_unit.Name} cannot move!");
        }
    }
}
