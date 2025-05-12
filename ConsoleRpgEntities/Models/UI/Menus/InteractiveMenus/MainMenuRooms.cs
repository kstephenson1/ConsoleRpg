using ConsoleRpgEntities.Models.UI.Character;
using ConsoleRpgEntities.Services;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class MainMenuRooms : InteractiveMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private readonly RoomUtilities _roomUtilities;
    bool _exit = false;

    public MainMenuRooms(RoomUtilities roomUtilities)
    {
        _roomUtilities = roomUtilities;
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

    public virtual void Display()
    {
        _exit = false;
        while (_exit != true)
        {
            Console.Clear();
            Update();
            Show();
            ConsoleKey key = ReturnValidKey();
            DoKeyAction(key, out _exit);
            if (_exit) break;
        }
    }

    public void Update()
    {
        _menuItems = new();
        AddMenuItem("Display Rooms", "Displays all rooms and their descriptions.", _roomUtilities.DisplayRooms);
        AddMenuItem("New Room", "Creates a new room.", _roomUtilities.CreateRoom);
        AddMenuItem("Edit Room", "Edits an existing room.", _roomUtilities.EditRoom);
        AddMenuItem("Find Room by Name", "Finds a room based on search.", _roomUtilities.FindRoomByName);
        AddMenuItem("Find Room by List", "Finds a room chosen by a list.", _roomUtilities.FindRoomByList);
        AddMenuItem("Go Back", "", End);
        BuildTable("");
    }

    protected override bool MenuSelectEnter()
    {
        Action(_selectedIndex);
        return _selectedIndex == _menuItems.Count - 1 ? true : false;
    }

    private void End()
    {
        _exit = true;
    }

    protected override void WaitForEnterPress()
    {
        if (_exit) return;
        base.WaitForEnterPress();
    }
}

