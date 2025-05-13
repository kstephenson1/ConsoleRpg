using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.Items.EquippableItems;

public abstract class EquippableItem : Item, IEquippableItem
{

    // EquippableItem is a class that holds equippable item information.

    public Rank RequiredRank { get; set; }
    public int Durability { get; set; }
    public int Weight { get; set; }
    public int ExpModifier { get; set; }

    protected EquippableItem() { }

    public EquippableItem(string name, string desc, Rank rank, int maxDurability, int weight, int expModifier)
        : base(name, desc)
    {
        MaxDurability = maxDurability;
        Name = name;
        RequiredRank = rank;
        Weight = weight;
        Durability = maxDurability;
        ExpModifier = expModifier;
    }

    public void TakeDurabilityDamage(int durabilityDamage)
    {
        throw new NotImplementedException();
    }

    public void BreakItem()
    {
        throw new NotImplementedException();
    }
}
