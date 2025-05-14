using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

namespace ConsoleRpgEntities.Models.UI;

// The UserInterface class contains a list of various menu items for easy access.
public class UserInterface
{
    public AbilitySelectionMenu AbilitySelectionMenu { get; private set; }
    public AbilityTypeSelectionMenu AbilityTypeSelectionMenu { get; private set; }
    public AttributeSelectionMenu AttributeSelectionMenu { get; private set; }
    public ClassTypeSelectionMenu ClassTypeSelectionMenu { get; private set; }
    public CommandMenu CommandMenu { get; private set; }
    public EnemyUnitSelectionMenu EnemyUnitSelectionMenu { get; private set; }
    public ItemCommandMenu ItemCommandMenu { get; private set; }
    public InventoryMenu InventoryMenu { get; private set; }
    public LevelUpCharacterMenu LevelUpCharacterMenu { get; private set; }
    public OperandSelectionMenu OperandSelectionMenu { get; private set; }
    public PartyUnitSelectionMenu PartyUnitSelectionMenu { get; private set; }
    public RoomMenu RoomMenu { get; private set; }
    public RoomNavigationMenu RoomNavigationMenu { get; private set; }
    public StatSelectionMenu StatSelectionMenu { get; private set; }


    public UserInterface(
        AbilitySelectionMenu abilitySelectionMenu,
        AttributeSelectionMenu attributeSelectionMenu,
        AbilityTypeSelectionMenu abilityTypeSelectionMenu,
        ClassTypeSelectionMenu classTypeSelectionMenu,
        CommandMenu commandMenu,
        EnemyUnitSelectionMenu enemyUnitSelectionMenu,
        ItemCommandMenu itemCommandMenu,
        InventoryMenu inventoryMenu,
        LevelUpCharacterMenu levelUpCharacterMenu,
        OperandSelectionMenu operandSelectionMenu,
        PartyUnitSelectionMenu partyUnitSelectionMenu,
        RoomMenu roomMenu,
        RoomNavigationMenu roomNavigationMenu,
        StatSelectionMenu statSelectionMenu)
    {
        AbilitySelectionMenu = abilitySelectionMenu;
        AbilityTypeSelectionMenu = abilityTypeSelectionMenu;
        AttributeSelectionMenu = attributeSelectionMenu;
        ClassTypeSelectionMenu = classTypeSelectionMenu;
        CommandMenu = commandMenu;
        EnemyUnitSelectionMenu = enemyUnitSelectionMenu;
        InventoryMenu = inventoryMenu;
        ItemCommandMenu = itemCommandMenu;
        LevelUpCharacterMenu = levelUpCharacterMenu;
        OperandSelectionMenu = operandSelectionMenu;
        PartyUnitSelectionMenu = partyUnitSelectionMenu;
        RoomMenu = roomMenu;
        RoomNavigationMenu = roomNavigationMenu;
        StatSelectionMenu = statSelectionMenu;
    }
}
