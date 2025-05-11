using ConsoleRpgEntities.Models.UI.Character;
using ConsoleRpgEntities.Services;
using ConsoleRpgEntities.Services.Repositories;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class MainMenu : InteractiveMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private CharacterUtilities _characterUtilities;
    private ItemService _itemService;
    private RoomFactory _roomFactory;
    private RoomUI _roomUI;
    private MainMenuInventory _mainMenuInventory;
    public MainMenu(CharacterUtilities characterUtilities, ItemService itemService, RoomFactory roomFactory, RoomUI roomUI, MainMenuInventory mainMenuInventory)
    {
        _characterUtilities = characterUtilities;
        _itemService = itemService;
        _roomFactory = roomFactory;
        _roomUI = roomUI;
        _mainMenuInventory = mainMenuInventory;
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
        AddMenuItem("Display Characters", "Displays all characters and items in their inventory.", _characterUtilities.DisplayCharacters);
        AddMenuItem("Edit Character", "Select a unit and change it's properties.", _characterUtilities.EditCharacter);
        AddMenuItem("Display Rooms", "Displays all rooms and their descriptions.", _roomUI.DisplayRooms);
        AddMenuItem("Find Character by Name", "Finds an existing character by name.", _characterUtilities.FindCharacterByName);
        AddMenuItem("Find Character by List", "Shows a list of units to select from", _characterUtilities.FindCharacterByList);
        AddMenuItem("New Character", "Creates a new character.", _characterUtilities.NewCharacter);
        AddMenuItem("New Room", "Creates a new room.", _roomFactory.CreateRoomAndAddToContext);
        AddMenuItem("Edit Character Level", "Level up/down a selected character.", _characterUtilities.LevelUp);
        AddMenuItem("Edit Character Level by List", "Level up/down a selected character.", _characterUtilities.LevelUpByList);
        AddMenuItem("Inventory Management", "Shows options for inventory management", _mainMenuInventory.Display);
        AddMenuItem("Add Ability to Character", "Adds an ability to a character", _characterUtilities.AddAbilityToCharacter);
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

