namespace ConsoleRpgEntities.DataTypes
{
    /// <summary>
    /// Represents the slot where an item can be equipped.
    /// '0' is used to represent unequipped items in the inventory.
    /// </summary>
    public enum EquipmentSlot
    {
        None = 0,
        Weapon = 1,
        Head = 2,
        Chest = 3,
        Legs = 4,
        Feet = 5,
    }
}
