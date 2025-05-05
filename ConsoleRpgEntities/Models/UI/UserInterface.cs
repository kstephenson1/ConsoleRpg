using ConsoleRpgEntities.Models.UI.Menus;
using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

namespace ConsoleRpgEntities.Models.UI;

// The UserInterface class contains a list of various menu items for easy access.
public class UserInterface
{
    public CommandMenu CommandMenu { get; private set; }
    public ExitMenu ExitMenu { get; private set; }
    public ItemCommandMenu ItemCommandMenu { get; private set; } = new();
    public InventoryMenu InventoryMenu { get; private set; } = new();
    public MainMenu MainMenu { get; private set; }
    public MainMenuInventory MainMenuInventory { get; private set; }
    public UnitSelectionMenu UnitSelectionMenu { get; private set; }
    public UnitClassMenu UnitClassMenu { get; private set; }

    public UserInterface(MainMenu mainMenu, MainMenuInventory mainMenuInventory, UnitSelectionMenu unitSelectionMenu, ExitMenu exitMenu, CommandMenu commandMenu, InventoryMenu inventoryMenu, ItemCommandMenu itemCommandMenu, UnitClassMenu unitClassMenu)
    {
        CommandMenu = commandMenu;
        ExitMenu = exitMenu;
        ItemCommandMenu = itemCommandMenu;
        InventoryMenu = inventoryMenu;
        MainMenu = mainMenu;
        MainMenuInventory = mainMenuInventory;
        UnitSelectionMenu = unitSelectionMenu;
        UnitClassMenu = unitClassMenu;
    }
}
