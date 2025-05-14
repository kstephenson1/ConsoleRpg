using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Dungeons;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.UI;

namespace ConsoleRpgEntities.Services;

public class DungeonFactory
{
    private readonly RoomFactory _roomFactory;
    private readonly UserInterface _ui;

    public DungeonFactory(RoomFactory roomFactory, UserInterface userInterface)
    {
        _roomFactory = roomFactory;

    }
    public Dungeon CreateDungeon(string dungeonName)
    {
        switch (dungeonName)
        {
            case "intro":
                Dungeon dungeon = new Dungeon(_ui.RoomNavigationMenu);
                Room entrance = _roomFactory.CreateRoom("intro.entrance");
                Room jail = _roomFactory.CreateRoom("intro.jail");
                Room kitchen = _roomFactory.CreateRoom("intro.kitchen");
                Room hallway = _roomFactory.CreateRoom("intro.hallway");
                Room library = _roomFactory.CreateRoom("intro.entrance");
                Room dwelling = _roomFactory.CreateRoom("intro.dwelling");
                Room dwelling2 = _roomFactory.CreateRoom("intro.dwelling2");
                entrance.AddAdjacentRoom(jail, Direction.West);
                entrance.AddAdjacentRoom(kitchen, Direction.East);
                entrance.AddAdjacentRoom(hallway, Direction.North);
                hallway.AddAdjacentRoom(dwelling2, Direction.West);
                hallway.AddAdjacentRoom(library, Direction.East);
                hallway.AddAdjacentRoom(dwelling, Direction.North);

                dungeon.StartingRoom = entrance;
                return dungeon;
            default:
                throw new ArgumentOutOfRangeException($"DungeonFactory tried to create dungeon and it does not exist: {dungeonName}");
        }
    }
}
