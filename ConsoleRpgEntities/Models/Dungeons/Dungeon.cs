using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;
using ConsoleRpgEntities.Models.Interfaces.Rooms;
using ConsoleRpgEntities.Services.Repositories;

namespace ConsoleRpgEntities.Models.Dungeons;

public class Dungeon : IDatabaseEntity
{
    // Dungeon is a class that represents a dungeon in the game. It contains a collection of rooms, a starting room,
    // and a navigation menu for the player to navigate through the dungeon. The player can enter the dungeon and
    // navigate through the rooms using the navigation menu.
    public int Id { get; set; }
    private RoomNavigationMenu _roomNavigationMenu;
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual Room StartingRoom { get; set; }
    public Dungeon() { }
    public Dungeon(RoomNavigationMenu roomNavigationMenu)
    {
        _roomNavigationMenu = roomNavigationMenu;
    }

    public void EnterDungeon()
    {
        IRoom currentRoom = StartingRoom;
        while (true)
        {
            currentRoom = _roomNavigationMenu.Display(currentRoom, "NavigationMenu", "Go Back");
        }
    }
}
