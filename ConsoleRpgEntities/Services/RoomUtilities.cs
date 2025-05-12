namespace ConsoleRpgEntities.Services;

using ConsoleRpgEntities.Models.Interfaces.Rooms;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.UI.Character;
using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;
using ConsoleRpgEntities.Services.DataHelpers;
using ConsoleRpgEntities.Services.Repositories;
using Spectre.Console;

public class RoomUtilities
{
    private RoomMenu _roomMenu;
    private RoomService _roomService;
    private RoomUI _roomUi;
    // RoomUtilities class contains fuctions that manipulate rooms based on user input.

    public RoomUtilities(RoomMenu roomMenu, RoomService roomService, RoomUI roomUi)
    {
        _roomMenu = roomMenu;
        _roomService = roomService;
        _roomUi = roomUi;
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
        IRoom room = _roomMenu.Display("Select a room to edit.", "[[Cancel Room Edit]]");
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
        Room room = (Room)_roomMenu.Display("Select room to view.", "[[Cancel Room Search]]");

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

    //public void AddAbilityToCharacter() // Adds an ability to a character.
    //{
    //    Type abilityType = _abilitySelectionMenu.Display("Select an ability to add.", "[[Cancel Ability Selection]]");
    //    if (abilityType == null) return;
    //    IUnit unit = _partyUnitSelectionMenu.Display($"Select unit to add the {abilityType.Name} ability to.", "[[Cancel Ability Selection]]");
    //    if (unit == null) return;

    //    Ability ability = (Ability)Activator.CreateInstance(abilityType);

    //    if (unit is Unit)
    //    {
    //        Unit character = (Unit)unit;
    //        character.Abilities.Add(ability);
    //        _unitService.Update(character);
    //        _unitService.Commit();
    //        AnsiConsole.MarkupLine($"Added ability [#00ffff]{ability.Name}[/] to [#00ffff]{unit.Name}[/]");
    //    }
    //}

    //public void DisplayAbilitiesForUnit()
    //{
    //    IUnit unit = _partyUnitSelectionMenu.Display("Select a unit to view it's abilities", "[[Go Back]]");
    //    if (unit == null) return;
    //    if (unit.Abilities.Any())
    //    {
    //        Console.WriteLine($"Abilities usable by {unit.Name}:");
    //        foreach (var ability in unit.Abilities)
    //        {
    //            Console.WriteLine($"{ability.Name} | {ability.Description}");
    //        }
    //    }
    //    else
    //    {
    //        Console.WriteLine($"{unit.Name} has no abilities.");
    //    }
    //}
}
