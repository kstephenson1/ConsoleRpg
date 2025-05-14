using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;
using ConsoleRpgEntities.Models.Items;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class InventoryMenu : InteractiveSelectionMenu<IItem>
{
    // InventoryMenu is used to display a unit's inventory.  It takes in a unit and a prompt, and displays the
    // unit's inventory then returns the selected item or null if the user exits the menu.

    public override void Display(string errorMessage)
    {
        throw new ArgumentException("CommandMenu(unit, prompt) requires a unit.");
    }

    public IItem Display(IUnit unit, string prompt, string exitMessage)
    {
        IItem selection = default!;
        bool exit = false;
        while (exit != true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Update(unit, exitMessage);
            BuildTable();
            Show();
            ConsoleKey key = ReturnValidKey();
            selection = DoKeyActionReturnUnit(key, out exit);
        }
        return selection;
    }

    protected override void Update(string exitMessage)
    {
        throw new ArgumentException("Update(item) requires an item.");
    }

    public void Update(IUnit unit, string exitMessage)
    {
        _menuItems = new();

        foreach (UnitItem unitItem in unit.UnitItems!)
        {
            if (unitItem.Item is IConsumableItem consumableItem)
            {
                AddMenuItem($"{consumableItem.Name}", $"[[{unitItem.Durability}/{consumableItem.MaxDurability}]] {consumableItem.Description}", unitItem.Item);
            }
            else if (unitItem.Item is IEquippableItem equippableItem)
            {
                if (InventoryHelper.IsItemEquipped(unit, equippableItem))
                {
                    AddMenuItem($"{equippableItem.Name}", $"[[E]] [[{equippableItem.Durability}/{equippableItem.MaxDurability}]] {equippableItem.Description}", unitItem.Item);
                }
                else
                {
                    AddMenuItem($"{equippableItem.Name}", $"[[{equippableItem.Durability}/{equippableItem.MaxDurability}]] {equippableItem.Description}", unitItem.Item);
                }
            }
            else
            {
                AddMenuItem(unitItem.Item.Name, unitItem.Item.Description, unitItem.Item);
            }
        }
        AddMenuItem(exitMessage, "", null!);
    }
}

