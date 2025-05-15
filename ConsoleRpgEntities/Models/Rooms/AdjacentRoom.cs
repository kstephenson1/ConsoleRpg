using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleRpgEntities.Models.Rooms;

[PrimaryKey("RoomId", "ConnectingRoomId")]
public class AdjacentRoom : IDatabaseEntity
{
    // UnitItem is a class that holds the properties of an item that is owned by a unit. It is used to store the
    // properties of an item that is owned by a unit.
    [NotMapped]
    public int Id => throw new NotImplementedException();

    [ForeignKey("Room")]
    public int RoomId { get; set; }
    public virtual Room Room { get; set; }

    [ForeignKey("ConnectingRoom")]
    public int ConnectingRoomId { get; set; }
    public virtual Room ConnectingRoom { get; set; }

    public virtual Direction Direction { get; set; } = Direction.None;

    public AdjacentRoom() { }

    public AdjacentRoom(Room room, Room connectingRoom, Direction direction)
    {
        Room = room;
        ConnectingRoom = connectingRoom;
        Direction = direction;
    }
}