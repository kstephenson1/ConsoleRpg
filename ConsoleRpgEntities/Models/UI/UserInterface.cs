using ConsoleRpgEntities.Models.UI.Menus;
using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

namespace ConsoleRpgEntities.Models.UI;

// The UserInterface class contains a list of various menu items for easy access.
public class UserInterface
{
    public AbilitySelectionMenu AbilitySelectionMenu { get; private set; }
    public CommandMenu CommandMenu { get; private set; }
    public ExitMenu ExitMenu { get; private set; }
    public ItemCommandMenu ItemCommandMenu { get; private set; }
    public InventoryMenu InventoryMenu { get; private set; }
    public MainMenu MainMenu { get; private set; }
    public MainMenuInventory MainMenuInventory { get; private set; }
    public PartyUnitSelectionMenu PartyUnitSelectionMenu { get; private set; }
    public EnemyUnitSelectionMenu EnemyUnitSelectionMenu { get; private set; }
    public ClassTypeSelectionMenu UnitClassMenu { get; private set; }

    public UserInterface(AbilitySelectionMenu abilitySelectionMenu, MainMenu mainMenu, MainMenuInventory mainMenuInventory, PartyUnitSelectionMenu partyUnitSelectionMenu, EnemyUnitSelectionMenu enemyUnitSelectionMenu, ExitMenu exitMenu, CommandMenu commandMenu, InventoryMenu inventoryMenu, ItemCommandMenu itemCommandMenu, ClassTypeSelectionMenu unitClassMenu)
    {
        AbilitySelectionMenu = abilitySelectionMenu;
        CommandMenu = commandMenu;
        EnemyUnitSelectionMenu = enemyUnitSelectionMenu;
        ExitMenu = exitMenu;
        ItemCommandMenu = itemCommandMenu;
        InventoryMenu = inventoryMenu;
        MainMenu = mainMenu;
        MainMenuInventory = mainMenuInventory;
        PartyUnitSelectionMenu = partyUnitSelectionMenu;
        UnitClassMenu = unitClassMenu;
    }
}
