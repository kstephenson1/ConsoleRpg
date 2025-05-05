using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Items.EquippableItems;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.Items.WeaponItems;

public abstract class WeaponItem : EquippableItem, IEquippableWeapon
{
    // WeaponItem is a class that holds weapon information.
    public override string ItemType { get; set; } = "WeaponItem";
    public WeaponType WeaponType { get; set; }
    public int Might { get; set; }
    public int Hit { get; set; }
    public int Crit { get; set; }
    public int Range { get; set; }

    public WeaponItem() { }
    public WeaponItem(string name, string desc, WeaponType type, Rank rank, int maxDurability, int might, int hit,
        int crit, int range, int weight, int expModifier)
        : base(name, desc, rank, maxDurability, weight, expModifier)
    {
        WeaponType = type;
        Might = might;
        Hit = hit;
        Crit = crit;
        Range = range;
    }
}
