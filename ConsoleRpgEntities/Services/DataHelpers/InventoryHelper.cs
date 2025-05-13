using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;
using ConsoleRpgEntities.Models.Items;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Services.Repositories;
using System.Security.Cryptography;

namespace ConsoleRpgEntities;

public class InventoryHelper
{
    private readonly ItemService _itemService;
    public InventoryHelper(ItemService itemService)
    {
        _itemService = itemService;
    }
    public static IEquippableWeapon? GetEquippedWeapon(IUnit unit)
    {
        if (unit.UnitItems == null) return null;
        foreach (UnitItem unitItem in unit.UnitItems)
        {
            if (unitItem.Item is IEquippableWeapon weaponItem && unitItem.Slot == EquipmentSlot.Weapon)
            {
                return weaponItem;
            }
        }
        return null;
    }

    public static IEquippableArmor? GetEquippedArmorInSlot(Unit unit, ArmorType armorType)
    {
        if (unit.UnitItems == null) return null;
        foreach (UnitItem unitItem in unit.UnitItems)
        {
            if (unitItem.Item is IEquippableArmor armorItem && unitItem.Slot == GetEquipmentSlotFromArmorType(armorType))
            {
                return armorItem;
            }
        }
        return null;
    }

    public static void EquipItem(IUnit unit, IEquippableItem equippableItem)
    {
        if(equippableItem is IEquippableWeapon weaponItem)
        {
            EquipItem(unit, weaponItem, EquipmentSlot.Weapon);
        }
        else if (equippableItem is IEquippableArmor armorItem)
        {
            EquipItem(unit, armorItem, GetEquipmentSlotFromArmorType(armorItem.ArmorType));
        }
    }

    public static void EquipItem(IUnit unit, IEquippableItem equippableItem, EquipmentSlot slot)
    {
        foreach (UnitItem unitItem in unit.UnitItems)
        {
            if (unitItem.Slot == slot)
            {
                unitItem.Slot = EquipmentSlot.None;
            }
            if (unitItem.Item == equippableItem)
            {
                unitItem.Slot = slot;
            }
        }
    }


    public static bool IsInventoryFull(IUnit unit)
    {
        if (unit.UnitItems.Count >= 6) return true;
        return false;
    }

    public static bool IsItemEquipped(IUnit unit, IEquippableItem equippableItem)
    {
        IEquippableWeapon? equippedWeapon = GetEquippedWeapon(unit);
        List<IEquippableArmor> equippedArmor = GetEquippedArmor(unit);

        foreach (UnitItem unitItem in unit.UnitItems)
        {
            if (equippableItem == equippedWeapon || equippedArmor.Contains(equippableItem))
            {
                return true;
            }
        }
        return false;
    }

    private static EquipmentSlot GetEquipmentSlotFromArmorType(ArmorType armorType)
    {
        return armorType switch
        {
            ArmorType.Head => EquipmentSlot.Head,
            ArmorType.Chest => EquipmentSlot.Chest,
            ArmorType.Legs => EquipmentSlot.Legs,
            ArmorType.Feet => EquipmentSlot.Feet,
            _ => throw new ArgumentOutOfRangeException($"Tried to get an equipment slot from an invalid armor type: {armorType}")
        };
    }

    public static List<Item> GetUnequippedItemsInInventory(IUnit unit)
    {
        List<Item> items = new();
        if (unit.UnitItems == null) return items;
        foreach (UnitItem unitItem in unit.UnitItems)
        {
            if (unitItem.Slot == EquipmentSlot.None)
            {
                items.Add(unitItem.Item);
            }
        }
        return items;
    }

    public static List<IEquippableArmor> GetEquippedArmor(IUnit unit)
    {
        List<IEquippableArmor> items = new();
        foreach (UnitItem unitItem in unit.UnitItems)
        {
            if (unitItem.Item is IEquippableArmor equippableArmor && unitItem.Slot != EquipmentSlot.None && unitItem.Slot != EquipmentSlot.Weapon)
            {
                items.Add(equippableArmor);
            }
        }
        return items;
    }

    public static bool CanCarryItem(IUnit unit, IItem item)
    {
        return unit.UnitItems.Count < 6;
    }

    public static void AddItemToInventory(IUnit unit, IItem item)
    {
        // Adds an item to the unit's inventory with max durability.
        AddItemToInventory(unit, item, EquipmentSlot.None);
    }

    public static void AddItemToInventory(IUnit unit, IItem item, EquipmentSlot slot)
    {
        // Adds an item to the unit's inventory in a specified slot with max durability.
        AddItemToInventory(unit, item, slot, item.MaxDurability);
    }

    public static void AddItemToInventory(IUnit unit, IItem item, EquipmentSlot slot, int durability)
    {
        // Adds an item to the unit's inventory in a specified slot with a specified durability.
        unit.UnitItems ??= new();

        unit.UnitItems.Add(new UnitItem
        {
            Item = (Item)item,
            ItemId = ((Item)item).Id,
            Unit = (Unit)unit,
            UnitId = unit.Id,
            Slot = slot,
            Durability = durability
        });
    }

    public static void RemoveItemFromInventory(IUnit unit, IItem item)
    {
        foreach(var unitItem in unit.UnitItems)
        {
            if (unitItem.Item == item)
            {
                unit.UnitItems.Remove(unitItem);
            }
        }
    }
}

