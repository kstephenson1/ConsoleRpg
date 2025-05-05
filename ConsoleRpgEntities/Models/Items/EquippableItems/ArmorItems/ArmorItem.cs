using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.Items.EquippableItems.ArmorItems;

public abstract class ArmorItem : EquippableItem, IEquippableArmor
{
    // ArmorItem is an abstract class that holds armor item information.
    public virtual ArmorType ArmorType { get; set; }
    public int Defense { get; set; }
    public int Resistance { get; set; }

    protected ArmorItem() { }

    public ArmorItem(string name, string desc, ArmorType type, Rank rank, int maxDurability, int defense, int resistance, int weight, int expModifier)
        : base(name, desc, rank, maxDurability, weight, expModifier)
    {
        ArmorType = type;
        Defense = defense;
        Resistance = resistance;
    }
}
