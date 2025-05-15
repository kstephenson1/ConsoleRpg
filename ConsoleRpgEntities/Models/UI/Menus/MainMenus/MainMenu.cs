using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;
using ConsoleRpgEntities.Models.UI.Menus.MenuItems;

namespace ConsoleRpgEntities.Models.UI.Menus.MainMenus;

public abstract class MainMenu : InteractiveMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    protected bool _exit = false;

    public MainMenu()
    {
        
    }

    public virtual void Display() => Display("[[Go Back]]");

    public override void Display(string exitMessage)
    {
        _exit = false;
        while (_exit != true)
        {
            Console.Clear();
            Update(exitMessage);
            Show();
            ConsoleKey key = ReturnValidKey();
            DoKeyAction(key, out _exit);
            if (_exit) break;
        }
    }

    protected void AddMenuItem(string name, string desc, Action action)
    {
        _menuItems.Add(new InteractiveSelectionMenuItem<Action>(_menuItems.Count, name, desc, action));
    }

    protected void Action(int selection)
    {
        // The Action method takes in a selecion from the main menu, then triggers the action associated with that menu item.
        List<InteractiveSelectionMenuItem<Action>> menuItems = new();

        foreach (MenuItem item in _menuItems) // Casts each of the MenuItems into MainMenuItems so the actions can work.
        {
            menuItems.Add((InteractiveSelectionMenuItem<Action>)item);
        }

        menuItems[selection].Selection(); // Runs the action selected.
    }

    protected override void Update(string exitMessage)
    {
        /* Example code block of a main menu update override
         * 
         * _menuItems = new();
         * AddMenuItem("Option 1", "Option 1 Example", Class.Action1);
         * AddMenuItem("Option 2", "Option 2 Example", Class.Action2);
         * AddMenuItem(exitMessage, "", End);
         * BuildTable();
         *
         */
    }

    protected override bool MenuSelectEnter()
    {
        Action(_selectedIndex);
        return _selectedIndex == _menuItems.Count - 1 ? true : false;
    }

    protected virtual void End()
    {
        _exit = true;
    }

    protected override void WaitForEnterPress()
    {
        if (_exit) return;
        base.WaitForEnterPress();
    }
}

