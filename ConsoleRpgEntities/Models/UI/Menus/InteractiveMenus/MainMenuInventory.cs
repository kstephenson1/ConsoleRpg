using ConsoleRpgEntities.Services.Repositories;
using Spectre.Console;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class MainMenuInventory : InteractiveMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private ItemService _itemService;
    bool _exit = false;
    public MainMenuInventory(ItemService itemService)
    {
        _itemService = itemService;
    }
    public void AddMenuItem(string name, string desc, Action action)
    {
        _menuItems.Add(new InteractiveSelectionMenuItem<Action>(_menuItems.Count, name, desc, action));
    }

    public void Action(int selection)
    {
        // The Action method takes in a selecion from the main menu, then triggers the action associated with that menu item.
        List<InteractiveSelectionMenuItem<Action>> menuItems = new();

        foreach (MenuItem item in _menuItems) // Casts each of the MenuItems into MainMenuItems so the actions can work.
        {
            menuItems.Add((InteractiveSelectionMenuItem<Action>)item);
        }

        menuItems[selection].Selection(); // Runs the action selected.
    }

    public virtual void Display()
    {
        _exit = false;
        while (_exit != true)
        {
            Console.Clear();
            Update();
            Show();
            ConsoleKey key = ReturnValidKey();
            DoKeyAction(key, out _exit);
            if (_exit) break;
        }
    }

    public void Update()
    {
        _menuItems = new();
        AddMenuItem("Search for Item by Name", "Finds a list of items based on search.", _itemService.SearchItemByName);
        AddMenuItem("List Items by Type", "Lists all items sorted by type.", _itemService.ListItemsByType);
        AddMenuItem("List Items Sorted by Name", "Lists all items in alphabetical order.", _itemService.ListItemsByName);
        AddMenuItem("List Weapons Sorted by Might", "Lists all weapons from weakest to strongest.", _itemService.ListItemsByMight);
        AddMenuItem("List Armor Sorted by Defense", "Lists all armors from weakest to strongest defense.", _itemService.ListItemsByDefense);
        AddMenuItem("List Armor Sorted by Resistance", "Lists all armors from weakest to stringest resistance.", _itemService.ListItemsByResistance);
        AddMenuItem("List Armor Sorted by Durability", "Lists all equippable items from least to most durability.", _itemService.ListItemsByDurability);
        AddMenuItem("Go Back", "", End);
        BuildTable("");
    }

    protected override bool MenuSelectEnter()
    {
        Action(_selectedIndex);
        return _selectedIndex == _menuItems.Count - 1 ? true : false;
    }

    private void End()
    {
        _exit = true;
    }

    protected override void WaitForEnterPress()
    {
        if (_exit) return;
        base.WaitForEnterPress();
    }
}

