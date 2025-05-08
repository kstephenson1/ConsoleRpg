using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;
using ConsoleRpgEntities.Models.Items;

namespace ConsoleRpgEntities.Models.Commands.ItemCommands;

public class DropItemCommand : ICommand
{
    // DropItemCommand is used to drop an item from a unit's inventory.  It takes in a unit and an item, and removes
    // the item from the unit's inventory. It also prints a message to the console indicating that the item has been
    // dropped.

    private readonly IUnit _unit;
    private readonly IItem _item;

    public DropItemCommand(IUnit unit, IItem item)
    {
        _unit = unit;
        _item = item;
    }
    public void Execute()
    {
        Console.WriteLine($"{_unit.Name} threw away {_item.Name}.");
        foreach (UnitItem unitItem in _unit.UnitItems)
        {
            if (unitItem.Item == _item)
            {
                _unit.UnitItems.Remove(unitItem);
                break;
            }
        }
    }
}
