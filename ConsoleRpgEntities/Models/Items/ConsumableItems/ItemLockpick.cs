using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.Items.ConsumableItems;

public class ItemLockpick : ConsumableItem, IConsumableItem
{
    // ItemLockpick is a consumable item that can be used to unlock doors or chests. It has a limited number of uses
    // and can be used by a unit. When the item is used, it decrements the number of uses left. If the number of uses
    // reaches zero, the item is removed from the unit's inventory.
    public override string ItemType { get; set; } = "ItemLockpick";

    public ItemLockpick()
    {
        Name = "Lockpick";
        Description = "Use to unlock a nearby door or chest.";
        MaxDurability = 5;
    }

    public ItemLockpick(string name, string desc) : base(name, desc)
    {
        MaxDurability = 5;
    }

    public void UseItem(IUnit unit)
    {
        Console.WriteLine($"{unit.Name} unlocked something nearby.");
        UnitItem unitItem = unit.UnitItems.Where(ui => ui.Item == this).FirstOrDefault()!;

        unitItem.Durability--;

        if (unitItem.Durability == 0)
        {
            Console.WriteLine($"The lockpick breaks.");
            unit.UnitItems.Remove(unitItem);
        }
    }
}
