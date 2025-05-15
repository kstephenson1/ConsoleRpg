namespace ConsoleRpgEntities.Services;

using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Rooms;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.UI;
using ConsoleRpgEntities.Models.UI.Character;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Services.DataHelpers;
using ConsoleRpgEntities.Services.Repositories;
using Spectre.Console;

public class RoomUtilities
{
    // RoomUtilities is a class that contains functions that manipulate and navigate the rooms based on user input.
    private readonly CharacterUtilities _characterUtilities;
    private readonly RoomService _roomService;
    private readonly RoomUI _roomUi;
    private readonly UnitService _unitService;
    private readonly UserInterface _ui;

    public RoomUtilities(CharacterUtilities characterUtilities, RoomService roomService, RoomUI roomUi, UnitService unitService, UserInterface userInterface)
    {
        _characterUtilities = characterUtilities;
        _roomService = roomService;
        _roomUi = roomUi;
        _unitService = unitService;
        _ui = userInterface;
    }
    public void CreateRoom()
    {
        Console.Clear();
        Console.WriteLine(
            "-------------------" +
            "    CREATE ROOM    " +
            "-------------------\n");
        string name = Input.GetString("Enter Name for new room: ");
        string desc = Input.GetString($"Enter Description for room \"{name}\": ");
        Room room = new(name, desc);
        _roomService.Add(room);
        _roomService.Commit();
        Console.WriteLine($"Room \"{name}\" with description \"{desc}\" has been added to the game.");
    }

    public void EditRoom()
    {
        Console.Clear();
        Console.WriteLine(
            "-------------------" +
            "     EDIT ROOM     " +
            "-------------------\n");
        IRoom room = _ui.RoomMenu.Display("Select a room to edit.", "[[Cancel Room Edit]]");
        if (room != null)
        {
            string newName = Input.GetString($"Please enter a new name for {room.Name}. (Leave blank to keep name) ", false);
            if (newName != "")
            {
                Console.WriteLine($"Name changed from {room.Name} to {newName}");
                room.Name = newName;
            }
            else
            {
                Console.WriteLine($"{room.Name}'s name has not been changed.");
            }
            string newDesc = Input.GetString($"Please enter a new description for {room.Name}. (Leave blank to keep description) ", false);
            if (newDesc != "")
            {
                Console.WriteLine($"Description changed from {room.Description} to {newDesc}");
                room.Description = newDesc;
            }
            else
            {
                Console.WriteLine($"{room.Name}'s description has not been changed.");
            }
            _roomService.Update((room as Room)!);
            _roomService.Commit();
        }
    }

    public void FindRoomByName() // Asks the user for a name and displays a character based on input.
    {
        string roomName = Input.GetString("What is the name of the room you would like to search for? ");
        List<Room> rooms = _roomService.GetAll().Where(c => c.Name.ToLower().Contains(roomName.ToLower())).ToList();

        Console.Clear();

        if (rooms.Any())
        {
            _roomUi.DisplayRooms(rooms);
        }
        else
        {
            AnsiConsole.MarkupLine($"[Red]No rooms found with the name {roomName}\n[/]");
        }
    }

    public void FindRoomByList() // Asks the user for a name and displays a character based on input.
    {
        Room room = (Room)_ui.RoomMenu.Display("Select room to view.", "[[Cancel Room Search]]");

        Console.Clear();

        if (room != null)
        {
            _roomUi.DisplayRoom(room);
        }
        else
        {
            AnsiConsole.MarkupLine($"[White]Room search cancelled.\n[/]");
        }
    }

    private Room? FindCharacterByName(string name) // Finds and returns a character based on input.
    {
        Room room = _roomService.GetAll().Where(c => c.Name.ToUpper() == name.ToUpper()).FirstOrDefault()!;
        return room;
    }

    public void DisplayRooms()                       //Displays each c's information.
    {
        List<Room> rooms = _roomService.GetAll().ToList();

        Console.Clear();

        _roomUi.DisplayRooms(rooms);
    }

    public void Navigate()
    {
        IUnit unit = _characterUtilities.ReturnCharacterByList("Select a character to navigate rooms with. ");

        Console.Clear();
        Console.WriteLine(
            "-------------------" +
            "   NAVIGATE ROOMS   " +
            "-------------------\n");
        IRoom room = unit.CurrentRoom!;

        do
        {
            room = _ui.RoomNavigationMenu.Display(unit.CurrentRoom!, $"{unit.Name} finds themself in {unit.CurrentRoom.Description}", "[[Stay in this room]]");
            if (room == null) break;
            unit.CurrentRoom = room as Room;
            Console.WriteLine($"{unit.Name} moved to the room named \"{room.Name}\"");
            _roomUi.DisplayRoom((room as Room)!);
        } while (room != null);

        //_roomService.Update((room as Room)!);
        _roomService.Commit();

        //_unitService.Update((unit as Unit)!);
        _unitService.Commit();

        Console.WriteLine($"{unit.Name} is staying in the room named \"{unit.CurrentRoom.Name}\"");
        _roomUi.DisplayRoom((unit.CurrentRoom as Room)!);
    }
}
