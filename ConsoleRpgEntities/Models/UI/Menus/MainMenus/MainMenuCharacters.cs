using ConsoleRpgEntities.Services;

namespace ConsoleRpgEntities.Models.UI.Menus.MainMenus;

public class MainMenuCharacters : MainMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private CharacterUtilities _characterUtilities;

    public MainMenuCharacters(CharacterUtilities characterUtilities)
    {
        _characterUtilities = characterUtilities;
    }
    
    protected override void Update(string exitMessage)
    {
        _menuItems = new();
        AddMenuItem("New Character", "Creates a new character.", _characterUtilities.NewCharacter);
        AddMenuItem("Edit Character", "Select a unit and change it's properties.", _characterUtilities.EditCharacter);
        AddMenuItem("Find Character by Name", "Finds an existing character by name.", _characterUtilities.FindCharacterByName);
        AddMenuItem("Find Character by List", "Shows a list of units to select from", _characterUtilities.FindCharacterByList);
        AddMenuItem("Find Character by Attribute", "Search a character by name, class, stats, etc.", _characterUtilities.ListCharactersByAttribute);
        AddMenuItem("Edit Level of Character by Search", "Level up/down a selected character.", _characterUtilities.LevelUp);
        AddMenuItem("Edit Level of Character by List", "Level up/down a selected character.", _characterUtilities.LevelUpByList);
        AddMenuItem(exitMessage, "", End);
        BuildTable();
    }
}

