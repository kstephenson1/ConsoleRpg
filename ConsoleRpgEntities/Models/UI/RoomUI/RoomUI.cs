using Spectre.Console;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Services.Repositories;
using ConsoleRpgEntities.Models.Units.Abstracts;

namespace ConsoleRpgEntities.Models.UI.Character;

public class RoomUI
{
    // RoomUI helps display room information in a nice little table.

    private RoomService _roomService;
    public RoomUI(RoomService roomService)
    {
        _roomService = roomService;
    }

    public void DisplayRooms(List<Room> rooms) // Displays the rooms and their info.
    {
        // Creates a display table that contains all the other tables to create a nice little display.
        foreach (Room room in rooms)
        {
            DisplayRoom(room);
        }
    }

    public void DisplayRoom(Room room) // Displays the rooms and their info.
    {
        // Creates a display table that contains all the other tables to create a nice little display.
        Table displayTable = new Table();
        displayTable
            .AddColumn(new TableColumn("Room Name").Width(30))
            .AddColumn(new TableColumn("Room Description").Width(100));

        displayTable.AddRow(room.Name, room.Description);
        displayTable.AddRow("", "");
        if (room.Units.Any())
        {
            displayTable.AddRow("", "Units in room:");
            foreach (Unit unit in room.Units)
            {
                displayTable.AddRow(new Text(""), new Text(unit.Name));
            }
        }
        else
        {
            displayTable.AddRow("", "No units in this room");
        }
        // Displays the table to the user.
        AnsiConsole.Write(displayTable);
    }
}
