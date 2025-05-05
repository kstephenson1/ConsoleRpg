using ConsoleRpgEntities.DataTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleRpgEntities.Models.Items.EquippableItems.ArmorItems;

public class HeadArmorItem : ArmorItem
{
    // HeadArmorItem is a class that represents a head armor item.
    public override string ItemType { get; set; } = "HeadArmorItem";
    [Column("ArmorType")]
    public override ArmorType ArmorType { get; set; } = ArmorType.Head;
    public HeadArmorItem() : base() { }
    public HeadArmorItem(string name, string desc, ArmorType type, Rank rank, int maxDurability, int defense, int resistance, int weight, int expModifier)
        : base(name, desc, type, rank, maxDurability, defense, resistance, weight, expModifier)
    {

    }
}
