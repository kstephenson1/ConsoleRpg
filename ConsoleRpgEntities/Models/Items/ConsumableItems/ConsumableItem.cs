namespace ConsoleRpgEntities.Models.Items.ConsumableItems;

public abstract class ConsumableItem : Item
{
    // ConsumableItem is an abstract class that holds basic consumable item properties and functions.
    public virtual int MaxUses { get; set; }
    public virtual int UsesLeft { get; set; }
    protected ConsumableItem() { }

    protected ConsumableItem(string name, string desc) : base(name, desc) { }
}
