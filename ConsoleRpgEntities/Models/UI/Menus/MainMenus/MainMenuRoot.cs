using ConsoleRpgEntities.Services;

namespace ConsoleRpgEntities.Models.UI.Menus.MainMenus;

public class MainMenuRoot : MainMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private readonly CharacterUtilities _characterUtilities;
    private readonly MainMenuAbilities _mainMenuAbilities;
    private readonly MainMenuInventory _mainMenuInventory;
    private readonly MainMenuCharacters _mainMenuCharacters;
    private readonly MainMenuRooms _mainMenuRooms;
    public MainMenuRoot(CharacterUtilities characterUtilities, MainMenuAbilities mainMenuAbilities, MainMenuCharacters mainMenuCharacters, MainMenuInventory mainMenuInventory, MainMenuRooms mainMenuRooms)
    {
        _characterUtilities = characterUtilities;
        _mainMenuAbilities = mainMenuAbilities;
        _mainMenuCharacters = mainMenuCharacters;
        _mainMenuInventory = mainMenuInventory;
        _mainMenuRooms = mainMenuRooms;
    }

    protected override void Update(string exitMessage)
    {
        _menuItems = new();
        AddMenuItem("Display Characters", "Displays all characters and their stats.", _characterUtilities.DisplayCharacters);
        AddMenuItem("Character Options", "Shows options for character management", _mainMenuCharacters.Display);
        AddMenuItem("Ability Options", "Shows options for ability management", _mainMenuAbilities.Display);
        AddMenuItem("Dungeon/Room Options", "Shows options for dungeons and/or room management", _mainMenuRooms.Display);
        AddMenuItem("Inventory Options", "Shows options for inventory management", _mainMenuInventory.Display);
        AddMenuItem(exitMessage, "", End);
        BuildTable();
    }
}

