using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Models.Interfaces.Rooms;
using ConsoleRpgEntities.Services.Repositories;

namespace ConsoleRpgEntities.Models.Rooms;

public abstract class RoomBase : IRoom, IDatabaseEntity
{
    // RoomBase is an abstract class that holds basic room properties and functions.
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual List<Unit>? Units { get; set; } = new();
    public virtual List<AdjacentRoom> AdjacentRooms { get; set; }

    protected RoomBase(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void OnRoomEnter(Unit unit)
    {
        Console.WriteLine($"{unit.Name} entered {Description}");
        unit.CurrentRoom.Units.Remove(unit);
        unit.CurrentRoom = this as Room;
        Units.Add(unit);
    }

    public void JoinAdjacentRooms(Room room, Direction direction)
    {
        AdjacentRooms ??= new();
        AdjacentRooms.Add(new(this as Room, room, direction));
        room.AdjacentRooms ??= new();
        room.AdjacentRooms.Add(new(room, this as Room, GetOppositeDirection(direction)));
    }

    public void JoinOneWayAdjacentRooms(Room room, Direction direction)
    {
        AdjacentRooms ??= new();
        AdjacentRooms.Add(new(this as Room, room, direction));
    }

    private Direction GetOppositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.South,
            Direction.NorthNorthEast => Direction.SouthSouthWest,
            Direction.NorthEast => Direction.SouthWest,
            Direction.EastNorthEast => Direction.WestSouthWest,
            Direction.East => Direction.West,
            Direction.EastSouthEast => Direction.WestNorthWest,
            Direction.SouthEast => Direction.NorthWest,
            Direction.SouthSouthEast => Direction.NorthNorthWest,
            Direction.South => Direction.North,
            Direction.SouthSouthWest => Direction.NorthNorthEast,
            Direction.SouthWest => Direction.NorthEast,
            Direction.WestSouthWest => Direction.EastNorthEast,
            Direction.West => Direction.East,
            Direction.WestNorthWest => Direction.EastSouthEast,
            Direction.NorthWest => Direction.SouthEast,
            Direction.NorthNorthWest => Direction.SouthSouthEast,
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Teleport => Direction.Teleport,
            Direction.None => Direction.None,
            _ => throw new ArgumentOutOfRangeException($"Direction does not exist: {direction.ToString()}")
        };
    }
}
