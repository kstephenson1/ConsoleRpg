using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Items.ConsumableItems;
using ConsoleRpgEntities.Models.Items.EquippableItems;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleRpgEntities.Models.Items;

[PrimaryKey("UnitId", "ItemId")]
public class UnitItem : IDatabaseEntity
{
    // UnitItem is a class that holds the properties of an item that is owned by a unit. It is used to store the
    // properties of an item that is owned by a unit.
    [NotMapped]
    public int Id => throw new NotImplementedException();

    [ForeignKey("Unit")]
    public int UnitId { get; set; }
    public virtual Unit Unit { get; set; }

    [ForeignKey("Item")]
    public int ItemId { get; set; }
    public virtual Item Item { get; set; }
    public virtual EquipmentSlot Slot { get; set; } = EquipmentSlot.None;
    public virtual int Durability { get; set; }

    public override string ToString()
    {
        return Item switch
        {
            EquippableItem => $"[[{Durability}/{Item.MaxDurability}]] {Item.Name}",
            ConsumableItem => $"[[{Durability}]] {Item.Name}",
            _ => $"{Item.Name}"
        };
    }
}