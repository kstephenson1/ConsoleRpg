using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.Items.ConsumableItems;

public class ItemBook : ConsumableItem, IConsumableItem
{
    // ItemBook is a consumable item that can be used by a unit.  It doesn't have any effects on the unit, but it can
    // be used to read a book. It has a maximum number of uses, and when the uses are depleted, the item is no longer
    // usable. It also has a name and description.

    public override string ItemType { get; set; } = "ItemBook";
    public override int MaxUses { get; set ; } = 10;
    public ItemBook()
    {
        Name = "Book";
        Description = "Use to read the book.";
        Weight = 1;
        MaxUses = 10;
        UsesLeft = MaxUses;
    }

    public ItemBook(string name, string desc) : base(name, desc)
    {
        MaxUses = 10;
        UsesLeft = MaxUses;
    }

    public void UseItem(IUnit unit)
    {
        Console.WriteLine($"You read the book. Isn't there a battle going on right now!?");
    }

    public override string ToString()
    {
        return $"{Name} ({UsesLeft}/{MaxUses})";
    }
}
