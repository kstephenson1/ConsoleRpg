using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Services.Repositories;

namespace ConsoleRpgEntities.Models.Items;

public abstract class Item : IItem, IDatabaseEntity
{
    // Item is an abstract class that represents an item in the game. It has a name, description, and a list of units
    // to establish a relationship between the item and the unit.
    public int Id { get; set; }
    public abstract string ItemType { get; set; }
    public virtual List<Unit> Units { get; set; }
    public virtual List<UnitItem> UnitItems { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int MaxDurability { get; set; }

    public Item() { }

    public Item(string name, string desc)
    {
        Name = name;
        Description = desc;
    }
}
