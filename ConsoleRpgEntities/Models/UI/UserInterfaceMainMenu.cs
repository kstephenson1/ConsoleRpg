using ConsoleRpgEntities.Models.UI.Menus;
using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

namespace ConsoleRpgEntities.Models.UI;

// The UserInterface class contains a list of various menu items for easy access.
public class UserInterfaceMainMenu
{
    public ExitMenu Exit { get; private set; }
    public MainMenu MainMenu { get; private set; }
    public MainMenuAbilities Abilities { get; private set; }
    public MainMenuCharacters Characters { get; private set; }
    public MainMenuInventory Inventory { get; private set; }
    public MainMenuRooms Rooms { get; private set; }


    public UserInterfaceMainMenu(ExitMenu exitMenu, MainMenu mainMenu, MainMenuInventory mainMenuInventory, MainMenuAbilities mainMenuAbilities, MainMenuCharacters mainMenuCharacters, MainMenuRooms mainMenuRooms)
    {
        Exit = exitMenu;
        MainMenu = mainMenu;
        Inventory = mainMenuInventory;
        Abilities = mainMenuAbilities;
        Characters = mainMenuCharacters;
        Rooms = mainMenuRooms;
    }
}
