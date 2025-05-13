using ConsoleRpgEntities.DataTypes;

namespace ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

public interface IEquippableItem : IItem
{
    // Interface that allows items to be equipped by units.
    public Rank RequiredRank { get; set; }
    public int MaxDurability { get; set; }
    public int Durability { get; set; }
    public int Weight { get; set; }
    public int ExpModifier { get; set; }
    public void TakeDurabilityDamage(int durabilityDamage);
    public void BreakItem();
}
