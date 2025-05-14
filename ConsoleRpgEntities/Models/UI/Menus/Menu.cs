using ConsoleRpgEntities.Models.UI.Menus.MenuItems;
using Spectre.Console;

namespace ConsoleRpgEntities.Models.UI.Menus;

public abstract class Menu
{
    // The Menus class is an abstract(ish) class to build other menus off of.  The Menus class holds a table which is
    // part of the user interface which is displayed to the user.  The Menu also holds menu items, which can store
    // different types of data.  It can be used by itself if you want a simple message box.

    protected Table _table = new();
    protected List<MenuItem> _menuItems = new();

    public virtual void AddMenuItem(string name) // Adds a new menu item to the menu.
    {
        _menuItems.Add(new MenuItem(_menuItems.Count + 1, name));
    }

    protected virtual void BuildTable() // Builds and stores a custom table for the menu using the menu items stored.
    {
        _table.AddColumn("Header");

        foreach (MenuItem item in _menuItems)
        {
            _table.AddRow(item.Name);
        }
        _table.HideHeaders();
    }

    public virtual void Show() // Shows the menu (Shows the table)
    {
        AnsiConsole.Write(_table);
    }
}

