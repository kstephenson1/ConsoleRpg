using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.Commands.ItemCommands;

public class EquipCommand : ICommand
{
    // EquipCommand is used to equip an item to a unit.  It takes in a unit and an item, and equips the item to the
    // unit's inventory. It also prints a message to the console indicating that the item has been equipped. 

    private readonly IUnit _unit;
    private readonly IEquippableItem _item;
    public EquipCommand(IUnit unit, IEquippableItem item)
    {
        _unit = unit;
        _item = item;
    }
    public void Execute()
    {
        Console.WriteLine($"{_unit.Name} equipped {_item.Name}");
        InventoryHelper.EquipItem(_unit, _item);
    }
}
