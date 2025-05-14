using ConsoleRpgEntities.Services;
using ConsoleRpgEntities.Services.Repositories;
using Spectre.Console;

namespace ConsoleRpgEntities.Models.UI.Menus.MainMenus;

public class MainMenuInventory : MainMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private CharacterUtilities _characterUtilities;
    private ItemService _itemService;

    public MainMenuInventory(CharacterUtilities characterUtilities, ItemService itemService)
    {
        _characterUtilities = characterUtilities;
        _itemService = itemService;
    }

    protected override void Update(string exitMessage)
    {
        _menuItems = new();
        AddMenuItem("Search for Item by Name", "Finds a list of items based on search.", _itemService.SearchItemByName);
        AddMenuItem("List Items by Type", "Lists all items sorted by type.", _itemService.ListItemsByType);
        AddMenuItem("List Items Sorted by Name", "Lists all items in alphabetical order.", _itemService.ListItemsByName);
        AddMenuItem("List Weapons Sorted by Might", "Lists all weapons from weakest to strongest.", _itemService.ListItemsByMight);
        AddMenuItem("List Armor Sorted by Defense", "Lists all armors from weakest to strongest defense.", _itemService.ListItemsByDefense);
        AddMenuItem("List Armor Sorted by Resistance", "Lists all armors from weakest to stringest resistance.", _itemService.ListItemsByResistance);
        AddMenuItem("List Armor Sorted by Durability", "Lists all equippable items from least to most durability.", _itemService.ListItemsByDurability);
        AddMenuItem("Find units with Item", "Enter item name and search for all characters with that item.", _characterUtilities.DisplayUnitsWithItem);
        AddMenuItem(exitMessage, "", End);
        BuildTable();
    }
}

