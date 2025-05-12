using ConsoleRpgEntities.Services;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class MainMenuAbilities : InteractiveMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private CharacterUtilities _characterUtilities;
    bool _exit = false;
    public MainMenuAbilities(CharacterUtilities characterUtilities)
    {
        _characterUtilities = characterUtilities;
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
        AddMenuItem("View Abilities for Character", "Shows performable abilities by character.", _characterUtilities.DisplayAbilitiesForUnit);
        AddMenuItem("Add Ability to Character", "Adds an ability to a character", _characterUtilities.AddAbilityToCharacter);
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

