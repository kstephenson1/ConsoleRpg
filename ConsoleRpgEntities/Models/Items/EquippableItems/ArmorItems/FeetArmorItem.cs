using ConsoleRpgEntities.DataTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleRpgEntities.Models.Items.EquippableItems.ArmorItems;

public class FeetArmorItem : ArmorItem
{
    // FeetArmorItem is a class that represents an item that can be equipped to the feet of a unit.
    public override string ItemType { get; set; } = "FeetArmorItem";
    [Column("ArmorType")]
    public override ArmorType ArmorType { get; set; } = ArmorType.Feet;
    public FeetArmorItem() : base() { }
    public FeetArmorItem(string name, string desc, ArmorType type, Rank rank, int maxDurability, int defense, int resistance, int weight, int expModifier)
        : base(name, desc, type, rank, maxDurability, defense, resistance, weight, expModifier)
    {

    }
}
