using ConsoleRpgEntities.DataTypes;

namespace ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

public interface IEquippableWeapon : IEquippableItem
{
    // IEquippableWeapon is an interface that defines the properties and methods for equippable weapon items.
    public WeaponType WeaponType { get; set; }
    public int Might { get; set; }
    public int Hit { get; set; }
    public int Crit { get; set; }
    public int Range { get; set; }
}
