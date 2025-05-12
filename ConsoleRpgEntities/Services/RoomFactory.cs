using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Services.DataHelpers;
using ConsoleRpgEntities.Services.Repositories;

namespace ConsoleRpgEntities.Services;

public class RoomFactory
{
    private RoomService _roomService;
    public RoomFactory(RoomService roomService)
    {
        _roomService = roomService;
    }

    public Room CreateRoom(string roomType)
    {
        return roomType switch
        {
            "intro.entrance" => new Room("Entrance", "the entryway to a seemingly small dungeon."),
            "intro.armory" => new Room("Armory", "a room with racks on the wall full or weapons, and manequins equipped with various types of armor."),
            "intro.kitchen" => new Room("Kitchen", "a room with a cooking pot used for cooking.  There are various vegetables and meats hanging from the ceiling."),
            "intro.hallway" => new Room("Hallway", "a mostly empty hallway that leads in four diretions."),
            "intro.jail" => new Room("Jail", "a room filled with jail cells.  One of them is locked."),
            "intro.library" => new Room("Library", "a room filled with empty bookshelves.  Goblins are not that big on reading."),
            "intro.dwelling" => new Room("Dwelling", "a small bedroom with a simple bed and a locked chest."),
            "intro.dwelling2" => new Room("Dwelling", "a small bedroom with a makeshift bed, scatted trash, and tattered clothing."),
        };
    }
}