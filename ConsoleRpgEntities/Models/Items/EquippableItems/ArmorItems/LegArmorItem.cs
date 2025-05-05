using ConsoleRpgEntities.DataTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleRpgEntities.Models.Items.EquippableItems.ArmorItems;

public class LegArmorItem : ArmorItem
{
    // LegArmorItem is a class that represents a leg armor item.
    public override string ItemType { get; set; } = "LegArmorItem";
    [Column("ArmorType")]
    public override ArmorType ArmorType { get; set; } = ArmorType.Legs;
    public LegArmorItem() : base() { }
    public LegArmorItem(string name, string desc, ArmorType type, Rank rank, int maxDurability, int defense, int resistance, int weight, int expModifier)
        : base(name, desc, type, rank, maxDurability, defense, resistance, weight, expModifier)
    {

    }
}
