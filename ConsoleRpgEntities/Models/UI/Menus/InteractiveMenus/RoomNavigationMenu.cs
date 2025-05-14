using ConsoleRpgEntities.Models.Interfaces.Rooms;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class RoomNavigationMenu : InteractiveSelectionMenu<IRoom>
{
    // The RoomNavigationMenu is used to navigate through the rooms in the game.  It takes in a room and a prompt, and
    // displays the rooms that are adjacent to the room. It returns the room that is selected by the user or null if the
    // user exits the menu.

    public override void Display(string errorMessage)
    {
        throw new ArgumentException("CommandMenu(unit, prompt) requires a unit.");
    }

    public IRoom Display(IRoom room, string prompt, string exitMessage)
    {
        IRoom selection = default!;
        bool exit = false;
        while (exit != true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Update(room, exitMessage);
            BuildTable();
            Show();
            ConsoleKey key = ReturnValidKey();
            selection = DoKeyActionReturnUnit(key, out exit);
        }
        return selection;
    }

    protected override void Update(string exitMessage)
    {
        throw new ArgumentException("Update(item) requires an item.");
    }

    public void Update(IRoom room, string exitMessage)
    {
        _menuItems = new();

        //foreach (AdjacentRoom adjacentRoom in room.AdjacentRooms)
        //{
        //    AddMenuItem($"{adjacentRoom.Direction.ToString()}", $"{adjacentRoom.Adjacent.Name}", adjacentRoom.Adjacent);
        //}

        AddMenuItem(exitMessage, "", null!);
    }
}

