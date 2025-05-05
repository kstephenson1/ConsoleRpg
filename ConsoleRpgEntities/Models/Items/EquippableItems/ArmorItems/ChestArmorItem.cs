using ConsoleRpgEntities.DataTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleRpgEntities.Models.Items.EquippableItems.ArmorItems;

public class ChestArmorItem : ArmorItem
{
    // ChestArmorItem represents an item that can be equipped in the chest slot.
    public override string ItemType { get; set; } = "ChestArmorItem";
    [Column("ArmorType")]
    public override ArmorType ArmorType { get; set; } = ArmorType.Chest;
    public ChestArmorItem() : base() { }
    public ChestArmorItem(string name, string desc, ArmorType type, Rank rank, int maxDurability, int defense, int resistance, int weight, int expModifier)
        : base(name, desc, type, rank, maxDurability, defense, resistance, weight, expModifier)
    {

    }
}
