namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public abstract class InteractiveSelectionMenu<T> : InteractiveMenu
{

    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.

    public InteractiveSelectionMenu() { }

    public InteractiveSelectionMenu(int selectedIndex)
    {
        _selectedIndex = selectedIndex;
    }
    public void AddMenuItem(string name, string desc, T selection)
    {
        _menuItems.Add(new InteractiveSelectionMenuItem<T>(_menuItems.Count, name, desc, selection));
    }

    public virtual T Display(string prompt, string exitMessage)
    {
        T selection = default!;
        bool exit = false;
        while (exit != true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Update(exitMessage);
            Show();
            ConsoleKey key = ReturnValidKey();
            selection = DoKeyActionReturnUnit(key, out exit);
        }
        return selection;
    }

    public T GetSelection(int selection)
    {
        // The Action method takes in a selecion from the main menu, then triggers the action associated with that menu item.
        List<InteractiveSelectionMenuItem<T>> menuItems = new();

        foreach (MenuItem item in _menuItems) // Casts each of the MenuItems into MainMenuItems so the actions can work.
        {
            menuItems.Add((InteractiveSelectionMenuItem<T>)item);
        }

        return menuItems[selection].Selection; // Runs the action selected.
    }

    protected bool MenuSelectEnterReturnUnit(out T selection)
    {
        selection = GetSelection(_selectedIndex);
        return true;
    }
    protected T DoKeyActionReturnUnit(ConsoleKey key, out bool exit)
    {
        T selection = default!;
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
                exit = MenuSelectEnterReturnUnit(out selection);
                break;
        }
        return selection;
    }
}

