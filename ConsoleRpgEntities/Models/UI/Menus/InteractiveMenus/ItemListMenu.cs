using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Services.Repositories;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class ItemListMenu : InteractiveSelectionMenu<IItem>
{
    // ItemListMenu is used to display a list of items in the game.  It takes in a prompt and an _exit message, and
    // displays a list of items in the game and returns the selected item.
    private ItemService _itemService;

    public ItemListMenu(ItemService itemService)
    {
        _itemService = itemService;
    }

    public override void Display(string errorMessage)
    {
        throw new ArgumentException("CommandMenu(unit, prompt) requires a unit.");
    }

    public IItem Display(string prompt, string exitMessage)
    {
        IItem selection = default!;
        bool exit = false;
        while (exit != true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Update(exitMessage);
            BuildTable(exitMessage);
            Show();
            ConsoleKey key = ReturnValidKey();
            selection = DoKeyActionReturnUnit(key, out exit);
        }
        return selection;
    }

    public override void Update(string exitMessage)
    {
        _menuItems = new();
        List<IItem> items = [.. _itemService.GetAll().ToList()];

        foreach (IItem item in items)
        {
            AddMenuItem($"{item.Name}", $"{item.Description}", item);
        }

        AddMenuItem(exitMessage, "", null!);
    }
}


