using ConsoleRpgEntities.Models.UI.Character;
using ConsoleRpgEntities.Services;

namespace ConsoleRpgEntities.Models.UI.Menus.MainMenus;

public class MainMenuRooms : MainMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private readonly RoomUtilities _roomUtilities;

    public MainMenuRooms(RoomUtilities roomUtilities)
    {
        _roomUtilities = roomUtilities;
    }

    protected override void Update(string exitMessage)
    {
        _menuItems = new();
        AddMenuItem("Display Rooms", "Displays all rooms and their descriptions.", _roomUtilities.DisplayRooms);
        AddMenuItem("New Room", "Creates a new room.", _roomUtilities.CreateRoom);
        AddMenuItem("Edit Room", "Edits an existing room.", _roomUtilities.EditRoom);
        AddMenuItem("Find Room by Name", "Finds a room based on search.", _roomUtilities.FindRoomByName);
        AddMenuItem("Find Room by List", "Finds a room chosen by a list.", _roomUtilities.FindRoomByList);
        AddMenuItem("Navigate rooms", "Select a character and navigate the rooms", _roomUtilities.Navigate);
        AddMenuItem(exitMessage, "", End);
        BuildTable();
    }
}

