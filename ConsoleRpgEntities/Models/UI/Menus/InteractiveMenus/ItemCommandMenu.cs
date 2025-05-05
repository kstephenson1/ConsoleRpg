using ConsoleRpgEntities.Models.Commands.ItemCommands;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class ItemCommandMenu : InteractiveSelectionMenu<ICommand>
{
    // ItemCommandMenu is used to display a menu of commands that can be performed on an item.  It takes in a unit and an
    // item, and displays a menu of commands that can be performed on the item, then returns the command that was
    // selected or returns null if the user exits the menu.

    public override void Display(string errorMessage)
    {
        throw new ArgumentException("CommandMenu(unit, prompt) requires a unit.");
    }

    public ICommand Display(IUnit unit, IItem item, string prompt, string exitMessage)
    {
        ICommand selection = default!;
        bool exit = false;
        while (exit != true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Update(unit, item, exitMessage);
            BuildTable(exitMessage);
            Show();
            ConsoleKey key = ReturnValidKey();
            selection = DoKeyActionReturnUnit(key, out exit);
        }
        return selection;
    }

    public override void Update(string exitMessage)
    {
        throw new ArgumentException("Update(item) requires an item.");
    }

    public void Update(IUnit unit, IItem item, string exitMessage)
    {
        _menuItems = new();

        AddMenuItem($"Drop Item", $"Get rid of the item forever.", new DropItemCommand(null!, null!));
        AddMenuItem($"Trade Item", $"Gives this item to someone else.", new TradeItemCommand(null!, null!, null!));

        if (item is IConsumableItem consumableItem)
        {
            AddMenuItem($"Use Item", $"{consumableItem.Description}", new UseItemCommand(null!, null!));
        }

        if (item is IEquippableItem equippableItem)
        {
            if (InventoryHelper.IsItemEquipped(unit, equippableItem))
            {
                AddMenuItem($"Unequip Item", $"[[{equippableItem.Durability}/{equippableItem.MaxDurability}]] {equippableItem.Description}", new UnequipCommand(null!, null!));
            }
            else
            {
                AddMenuItem($"Equip Item", $"[[{equippableItem.Durability}/{equippableItem.MaxDurability}]] {equippableItem.Description}", new EquipCommand(null!, null!));
            }
        }
        AddMenuItem(exitMessage, "", null!);
    }
}

