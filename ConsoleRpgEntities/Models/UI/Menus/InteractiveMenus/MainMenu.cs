using ConsoleRpgEntities.Services;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class MainMenu : InteractiveMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private readonly CharacterUtilities _characterUtilities;
    private readonly MainMenuAbilities _mainMenuAbilities;
    private readonly MainMenuInventory _mainMenuInventory;
    private readonly MainMenuCharacters _mainMenuCharacters;
    private readonly MainMenuRooms _mainMenuRooms;
    public MainMenu(CharacterUtilities characterUtilities, MainMenuAbilities mainMenuAbilities, MainMenuCharacters mainMenuCharacters, MainMenuInventory mainMenuInventory, MainMenuRooms mainMenuRooms)
    {
        _characterUtilities = characterUtilities;
        _mainMenuAbilities = mainMenuAbilities;
        _mainMenuCharacters = mainMenuCharacters;
        _mainMenuInventory = mainMenuInventory;
        _mainMenuRooms = mainMenuRooms;
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

    public override void Update(string exitMessage)
    {
        _menuItems = new();
        AddMenuItem("Display Characters", "Displays all characters and their stats.", _characterUtilities.DisplayCharacters);
        AddMenuItem("Character Options", "Shows options for character management", _mainMenuCharacters.Display);
        AddMenuItem("Ability Options", "Shows options for ability management", _mainMenuAbilities.Display);
        AddMenuItem("Dungeon/Room Options", "Shows options for dungeons and/or room management", _mainMenuRooms.Display);
        AddMenuItem("Inventory Options", "Shows options for inventory management", _mainMenuInventory.Display);
        AddMenuItem(exitMessage, "", DoNothing);
        BuildTable(exitMessage);
    }

    protected override bool MenuSelectEnter()
    {
        Action(_selectedIndex);
        return _selectedIndex == _menuItems.Count - 1 ? true : false;
    }

    private void DoNothing() { } // This method does nothing...  or does it?
}

