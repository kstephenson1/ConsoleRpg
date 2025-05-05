using Spectre.Console;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public abstract class InteractiveMenu : Menu
{
    // The interactive menu is a menu where you can select the options by moving the arrow selector up and down
    // using the arrow or w/s keys. The MainMenu contains items that have 4 parts, the index, the name, the
    // description, and the action that is completed when that menu item is chosen.
    public InteractiveMenu()
    {

    }

    public InteractiveMenu(int selectedIndex)
    {
        _selectedIndex = selectedIndex;
    }

    protected int _selectedIndex = 0;
    public void AddMenuItem(string name, string desc)
    {
        _menuItems.Add(new InteractiveMenuItem(_menuItems.Count, name, desc));
    }

    public virtual void Display(string errorMessage)
    {
        bool exit = false;
        while (exit != true)
        {
            Console.Clear();
            Update(errorMessage);
            Show();
            ConsoleKey key = ReturnValidKey();
            DoKeyAction(key, out exit);
        }
    }

    protected override void BuildTable(string exitMessage)
    {
        _table = new();
        // Creates a table using Spectre.Console and stores that table for later.
        _table.AddColumn("#");
        _table.AddColumn("Selection");
        _table.AddColumn("Description");

        foreach (InteractiveMenuItem item in _menuItems)
        {
            _table.AddRow(GetSelectableArrow(item), item.Name, item.Description);
        }

        _table.HideHeaders();
    }

    protected string GetSelectableArrow(InteractiveMenuItem item)
    {
        if (_selectedIndex == item.Index)
        {
            return "->";
        }
        return " ";
    }

    protected virtual void MenuSelectUp()
    {
        if (_selectedIndex > 0) _selectedIndex--;
    }

    protected virtual void MenuSelectDown()
    {
        if (_selectedIndex < _menuItems.Count - 1) _selectedIndex++;
    }

    protected virtual bool MenuSelectEnter()
    {
        return _selectedIndex == _menuItems.Count - 1;
    }

    protected ConsoleKey ReturnValidKey()
    {
        while (true)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    return ConsoleKey.UpArrow;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    return ConsoleKey.DownArrow;
                case ConsoleKey.E or ConsoleKey.Enter:
                    return ConsoleKey.Enter;
                default:
                    break;
            }
        }
    }

    protected virtual void WaitForEnterPress()
    {
        AnsiConsole.MarkupLine($"Press [green][[ENTER]][/] to continue...");
        while (true)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Enter)
                return;
        }
    }

    protected virtual void DoKeyAction(ConsoleKey key, out bool exit)
    {
        exit = false;
        switch (key)
        {
            case ConsoleKey.UpArrow:
                MenuSelectUp();
                break;
            case ConsoleKey.DownArrow:
                MenuSelectDown();
                break;
            case ConsoleKey.Enter:
                exit = MenuSelectEnter();
                WaitForEnterPress();
                break;
        }
    }
    public virtual void Update(string exitMessage)
    {
        BuildTable(exitMessage);
    }
    public int GetSelectedIndex() => _selectedIndex;
}

