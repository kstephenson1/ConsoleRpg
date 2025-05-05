using ConsoleRpgEntities.DataTypes;

namespace ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

public interface IEquippableArmor : IEquippableItem
{
    // IEquippableArmor is an interface that defines the properties and methods for equippable armor items.
    public abstract ArmorType ArmorType { get; set; }
    public int Defense { get; set; }
    public int Resistance { get; set; }
}
