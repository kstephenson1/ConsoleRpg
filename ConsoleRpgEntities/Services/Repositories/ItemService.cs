using ConsoleRpgEntities.Models.Items;
using ConsoleRpgEntities.Models.Items.EquippableItems;
using ConsoleRpgEntities.Models.Items.EquippableItems.ArmorItems;
using ConsoleRpgEntities.Models.Items.WeaponItems;
using ConsoleRpgEntities.Services.DataHelpers;
using Spectre.Console;

namespace ConsoleRpgEntities.Services.Repositories;
public class ItemService : IService<Item>
{
    private readonly IRepository<Item> _repo;
    public ItemService(IRepository<Item> repo)
    {
        _repo = repo;
    }
    public void Add(Item item)
    {
        _repo.Add(item);
    }

    public void Add(List<Item> items)
    {
        foreach (Item item in items)
        {
            _repo.Add(item);
        }
    }

    public void Delete(Item item)
    {
        _repo.Delete(item);
    }

    public IEnumerable<Item> GetAll()
    {
        return _repo.GetAll();
    }

    public Item? GetById(int id)
    {
        return _repo.GetById(id);
    }

    public void Update(Item item)
    {
        _repo.Update(item);
    }

    public void Commit()
    {
        _repo.Commit();
    }

    public void SearchItemByName()
    {
        string search = Input.GetString("Enter the name of the item you want to search for: ");
        IEnumerable<Item> items = GetAll().Where(i => i.Name.ToLower().Contains(search.ToLower()));
        Table table = new();
        table.AddColumn("Item Name");

        if (items.Any())
        {
            AnsiConsole.MarkupLine("Items found:");
            foreach (Item item in items)
            {
                table.AddRow(item.Name);
            }
        }
        else
        {
            table.AddRow("No items found with that name.");
        }
        AnsiConsole.Write(table);
    }

    public void ListItemsByType()
    {
        IEnumerable<Item> items = GetAll().OrderBy(i => i.Name).OrderBy(i => i.ItemType);
        string itemType = string.Empty;
        Table table = new();
        table.AddColumns("Item Type", "Item Name");

        foreach (Item item in items)
        {
            table.AddRow(item.ItemType, item.Name);
        }
        AnsiConsole.Write(table);
    }

    public void ListItemsByName()
    {
        Table table = new();
        table.AddColumns("Item Name");
        foreach (Item item in GetAll().OrderBy(i => i.Name))
        {
            table.AddRow(item.Name);
        }
        AnsiConsole.Write(table);
    }

    public void ListItemsByMight()
    {
        IEnumerable<Item> allItems = GetAll().Where(i => i.ItemType.Contains("Weapon"));
        List<WeaponItem> weaponItems = new();
        Table table = new();
        table.AddColumns("Item Name", "Might");

        foreach (Item item in allItems)
        {
            weaponItems.Add((WeaponItem)item);
        }

        foreach (WeaponItem weapon in weaponItems.OrderBy(i => i.Name).OrderBy(i => i.Might))
        {
            table.AddRow(weapon.Name, $"+{weapon.Might.ToString()} MGT");
        }
        AnsiConsole.Write(table);
    }

    public void ListItemsByDefense()
    {
        IEnumerable<Item> allItems = GetAll().Where(i => i.ItemType.Contains("Armor"));
        List<ArmorItem> armorItems = new();
        Table table = new();
        table.AddColumns("Item Name", "Defense");

        foreach (Item item in allItems)
        {
            armorItems.Add((ArmorItem)item);
        }

        foreach (ArmorItem armor in armorItems.OrderBy(i => i.Name).OrderBy(i => i.Defense))
        {
            table.AddRow(armor.Name, $"+{armor.Defense.ToString()} DEF");
        }
        AnsiConsole.Write(table);
    }

    public void ListItemsByResistance()
    {
        IEnumerable<Item> allItems = GetAll().Where(i => i.ItemType.Contains("Armor"));
        List<ArmorItem> armorItems = new();
        Table table = new();
        table.AddColumns("Item Name", "Resistance");

        foreach (Item item in allItems)
        {
            armorItems.Add((ArmorItem)item);
        }

        foreach (ArmorItem armor in armorItems.OrderBy(i => i.Name).OrderBy(i => i.Resistance))
        {
            table.AddRow(armor.Name, $"+{armor.Resistance.ToString()} RES");
        }
        AnsiConsole.Write(table);
    }

    public void ListItemsByDurability()
    {
        IEnumerable<Item> allItems = GetAll();
        List<EquippableItem> equippableItems = new();
        Table table = new();
        table.AddColumns("Item Name", "Durability");

        foreach (Item item in allItems)
        {
            if (item is EquippableItem equippableItem)
            {
                equippableItems.Add(equippableItem);
            }
        }

        foreach (EquippableItem equippableItem in equippableItems.OrderBy(i => i.Name).OrderBy(i => i.Durability))
        {
            table.AddRow(equippableItem.Name, $"{equippableItem.Durability}/{equippableItem.MaxDurability}");
        }
        AnsiConsole.Write(table);
    }
}
