using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.Commands.ItemCommands;

public class UseItemCommand : ICommand
{
    // UseItemCommand is used to use an item from a unit.  It takes in a unit and an item, and uses the item on the
    // unit. It also prints a message to the console indicating that the item has been used. If the item is not usable,
    // a message is printed to the console indicating that the item is not usable.

    private readonly IUnit _unit;
    private readonly IItem _item;
    public UseItemCommand() { }
    public UseItemCommand(IUnit unit, IItem item)
    {
        _unit = unit;
        _item = item;
    }
    public void Execute()
    {
        if (_item is IConsumableItem consumableItem)
        {
            consumableItem.UseItem(_unit);
        }
        else
        {
            Console.WriteLine("This item is not usable.");
        }
    }
}
