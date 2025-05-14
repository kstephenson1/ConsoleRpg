using ConsoleRpgEntities.Services;

namespace ConsoleRpgEntities.Models.UI.Menus.MainMenus;

public class MainMenuAbilities : MainMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private CharacterUtilities _characterUtilities;

    public MainMenuAbilities(CharacterUtilities characterUtilities)
    {
        _characterUtilities = characterUtilities;
    }

    protected override void Update(string exitMessage)
    {
        _menuItems = new();
        AddMenuItem("View Abilities for Character", "Shows performable abilities by character.", _characterUtilities.DisplayAbilitiesForUnit);
        AddMenuItem("Add Ability to Character", "Adds an ability to a character", _characterUtilities.AddAbilityToCharacter);
        AddMenuItem(exitMessage, "", End);
        BuildTable();
    }
}

