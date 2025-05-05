using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.Commands.ItemCommands;

public class UnequipCommand : ICommand
{
    // UnequipCommand is used to unequip an item from a unit.  It takes in a unit and an item, and unequips the item
    // from the unit's inventory. It also prints a message to the console indicating that the item has been unequipped.

    private readonly IUnit _unit;
    private readonly IEquippableItem _item;
    public UnequipCommand(IUnit unit, IEquippableItem item)
    {
        _unit = unit;
        _item = item;
    }
    public void Execute()
    {
        Console.WriteLine($"{_unit.Name} unequipped {_item.Name}");
        InventoryHelper.EquipItem(_unit, _item, DataTypes.EquipmentSlot.None);
    }
}
