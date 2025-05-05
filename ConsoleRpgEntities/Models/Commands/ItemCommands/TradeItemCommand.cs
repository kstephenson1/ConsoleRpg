using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;
using ConsoleRpgEntities.Models.Interfaces.InventoryBehaviors;

namespace ConsoleRpgEntities.Models.Commands.ItemCommands;

public class TradeItemCommand : ICommand
{
    // TradeItemCommand is used to trade an item from one unit to another.  It takes in a unit, an item, and a target
    // unit, and removes the item from the unit's inventory and adds it to the target's inventory. It also prints a
    // message to the console indicating that the item has been traded. If the target's inventory is full, a message is
    // printed to the console indicating that the item could not be traded.

    private readonly IUnit _unit;
    private readonly IItem _item;
    private readonly IUnit _target;
    public TradeItemCommand(IUnit unit, IItem item, IUnit target)
    {
        _unit = unit;
        _item = item;
        _target = target;
    }
    public void Execute()
    {
        if (_unit is IHaveInventory && _target is IHaveInventory)
        {
            if (!InventoryHelper.IsInventoryFull(_target))
            {
                foreach (UnitItem unitItem in _unit.UnitItems)
                {
                    if (unitItem.Item == _item)
                    {
                        _unit.UnitItems.Remove(unitItem);
                        _target.UnitItems.Add(unitItem);
                        break;
                    }
                }
                Console.WriteLine($"{_unit.Name} traded {_item.Name} to {_target.Name}");
            }
            else
            {
                Console.WriteLine($"{_unit.Name} cound not trade {_item.Name} to {_target.Name}.");
                Console.WriteLine($"{_target.Name}'s inventory is full.");
            }
        }
    }
}
