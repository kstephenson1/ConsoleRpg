# FINAL PRESENTATION - ConsoleRPG EF Core Application
## Basic Required functionality:
### Add a new Character to the database
- Prompt the user to enter details for your character (e.g. Name, Health, Attack, and Defense).
- Save the updated record to the database.
###### Here is the method that creates a new character.  I had to be a little creative when making this method, because when you create a character, you can select a class such as cleric, fighter, knight, rogue, or wizard.  I could not find an easier way to create a new instance of a specific class based on input, so I have opted to making a dynamic object that stores all the character information throughout the character creation process.  You start by typing in the name of the new character, then the selection menu has you select a class from the list of available classes which returns a unit class Type.  This type is then used with Activator.CreateInstance to create an instance of the chosen type and store it in a dynamic object.  All of the properties can be edited and then the dynamic object is added to the database.  When the stats are created, the user is prompted to add stats to their character for each level obtained.  The character gets a random amount of stat increases (2-6 stats per level).
```csharp
    public void NewCharacter() // Creates a new character.  Asks for name, class, level, hitpoints, and items.
    {
        string name = Input.GetString("Enter your character's name: ");
        Type characterClass = _ui.ClassTypeSelectionMenu.Display($"Please select a class for {name}", "[[Cancel Character Creation]]");
        if (characterClass == null) return;
        int level = Input.GetInt("Enter your character's level: ", 1, Config.CHARACTER_LEVEL_MAX, $"character level must be 1-{Config.CHARACTER_LEVEL_MAX}");
    
        dynamic character = Activator.CreateInstance(characterClass);
        character.Name = name;
        character.Class = characterClass.Name;
        character.Level = level;
    
        Stat stat = _statFactory.CreateStat(character);
        character.Stat = stat;
    
        List<UnitItem> unitItems = new();
        character.UnitItems = unitItems;
    
    
        IRoom room = _ui.RoomMenu.Display($"Select room for {character.Name}","[[No Room]]");
        if (room != null)
        {
            character.CurrentRoom = (Room)room;
        }
    
        Console.Clear();
        Console.WriteLine($"\nWelcome, {name} the {characterClass.Name}! You are level {level} and have {stat.MaxHitPoints} health.\n");
    
        _statService.Add(character.Stat);
        _unitService.Add(character);
        _statService.Commit();
    }
```
### Edit an existing Character
- Allow users to update attributes like Health, Attack, and Defense.
- Save the updated record to the database.
###### The EditCharacter method is a pretty simple one.  It allows the user to select a character from a list, then change some basic properties about it, such as the name, level, and hit points value.
```csharp
  public void EditCharacter() // Asks the user for a name and displays a character based on input.
  {
      IUnit character = ReturnCharacterByList("Select unit to edit.");
      Console.Clear();
      if (character != null)
      {
          _characterUI.DisplayCharacterInfo(character);

          string newName = Input.GetString($"Please enter a new name for {character.Name}. (Leave blank to keep name) ", false);
          if (newName != "")
          {
              Console.WriteLine($"Name changed from {character.Name} to {newName}");
              character.Name = newName;
          }
          else
          {
              Console.WriteLine($"{character.Name}'s name has not been changed.");
          }
  
          int newLevel = Input.GetInt($"Please enter a new level for {character.Name}. (Enter 0 to keep level the same) ", 0, Config.CHARACTER_LEVEL_MAX, $"character level must be 1-{Config.CHARACTER_LEVEL_MAX}");
          if (newLevel != 0)
          {
              Console.WriteLine($"Level changed from {character.Level} to {newLevel}");
              character.Level = newLevel;
          }
          else
          {
              Console.WriteLine($"{character.Name}'s level has not been changed.");
          }
  
          int newHitPoints = Input.GetInt($"Please enter a new hitpoints for {character.Name}. (Enter 0 to keep hitpoints the same) ", 0, 60, $"hitpoints must be between 1 and {60}");
          if (newHitPoints != 0)
          {
              Console.WriteLine($"Hitpoints changed from {character.Stat.MaxHitPoints} to {newHitPoints}");
              character.Stat.MaxHitPoints = newHitPoints;
              character.Stat.HitPoints = newHitPoints;
          }
          else
          {
              Console.WriteLine($"{character.Name}'s hitpoints have not been changed.");
          }
  
          _unitService.Update((Unit)character);
          _unitService.Commit();
      }
  }
```
### Display all Characters
- Include any relevant details to your character
###### DisplayCharacters is method that finds all non enemy units and displays those characters' infos.
```csharp
public void DisplayCharacters()                       //Displays each c's information.
{
    Console.Clear();
    List<Unit> units = _unitService.GetAll().Where(u => !u.UnitType.Contains("Enemy")).ToList();

    _characterUI.DisplayCharacterInfo(units);
}
```
###### DisplayCharacterInfo(List) cycles through a list of units, finds their items and rooms, then displays each character information seperately.  This method was created before lazy loading was added to the project, so the code is a little unnecessarily complex.  This could be changed in the future to be a lot more simple.
```csharp
    public void DisplayCharacterInfo(List<Unit> units)
    {
        List<Room> rooms = _roomService.GetAll().ToList();

        foreach (Unit unit in units)
        {
            List<Item> characterItems = new();
            foreach (UnitItem unitItem in unit.UnitItems)
            {
                characterItems.Add(unitItem.Item);
            }
            Room? unitRoom;
            try
            {
                unitRoom = rooms.Where(r => r.Id == unit.CurrentRoom!.Id).FirstOrDefault()!;
            }
            catch { unitRoom = null; }
            DisplayCharacterInfo(unit, unit.Stat, characterItems, unitRoom!, unit.Abilities);
        }
    }
```
###### DisplayCharacterInfo(Non-List) creates a spectre.console grid that stores a bunch of other grids that mau store more grids.  This method builds a nice looking table that shows the information for the unit including Name, level, and class; current rooms; stats such as strength, magic, constitution, dexterity, speed, luck, defense, resistance, and movement; a list of abilities known by the unit; and a list of inventory items showing which items are equipped and their durability.  All of this information is gathered and stored in grids.  and these grids are stored in a collection of grids where it is written as a specter.console table.  Spectre.console is a really neat package.
```csharp
public void DisplayCharacterInfo(IUnit unit, Stat stat, List<Item> items, Room room, List<Ability> abilities) // Displays the character's info
{
    // Builds a character table with 2 lines: Name, Level and Class.
    Grid charTable = new Grid().Width(30).AddColumn();
    charTable
        .AddRow(new Text(unit.Name).Centered())
            .AddRow(new Text($"Level {unit.Level} {unit.Class}").Centered());

    // Builds an hp table that contains the health of the character
    Grid hpTable = new Grid().Width(25).AddColumn();
    hpTable
        .AddRow(new Text($"Hit Points:").Centered())
            .AddRow(new Text($"{stat.HitPoints}/{stat.MaxHitPoints}").Centered());

    //Creates a table with the unit's stats.
    Grid invHeader = new Grid().Width(30).AddColumns(3);
    invHeader
        .AddRow(
            new Text($" STR: {stat.Strength}").LeftJustified(),
            new Text($"MAG: {unit.Stat.Magic}").LeftJustified(),
            new Text($"CON: {unit.Stat.Constitution}").LeftJustified())
        .AddRow(
            new Text($" DEX: {stat.Dexterity}").LeftJustified(),
            new Text($"SPD: {unit.Stat.Speed}").LeftJustified(),
            new Text($"LCK: {unit.Stat.Luck}").LeftJustified())
        .AddRow(
            new Text($" DEF: {stat.Defense}").LeftJustified(),
            new Text($"RES: {unit.Stat.Resistance}").LeftJustified(),
            new Text($"MOV: {unit.Stat.Movement}").LeftJustified());


    // Creates an inventory table that lists all the items in the character's inventory.
    Grid invTable = new Grid();
    invTable.AddColumn();

    List<UnitItem> equippedInventory = new();

    //IEquippableWeapon? weapon = unit.GetEquippedWeapon();
    UnitItem weapon = unit.UnitItems.Where(ui => ui.Unit == unit && ui.Slot == EquipmentSlot.Weapon).FirstOrDefault()!;
    if (weapon != null)
    {
        equippedInventory.Add(weapon);
    }

    List<UnitItem> equippedArmor = unit.UnitItems.Where(
        ui => ui.Unit == unit &&
        ui.Slot == EquipmentSlot.Head ||
        ui.Slot == EquipmentSlot.Chest ||
        ui.Slot == EquipmentSlot.Legs ||
        ui.Slot == EquipmentSlot.Feet).ToList()!;

    foreach (UnitItem armor in equippedArmor)
    {
        equippedInventory.Add(armor);
    }

    if (equippedInventory.Any())
    {
        invTable.AddRow("Equipped Items: ");

        foreach (UnitItem unitItem in equippedInventory)
        {
            invTable.AddRow($"{unitItem.ToString()}");
        }
    }
    else
    {
        invTable.AddRow("Equipped Items: ---");
    }

        invTable.AddRow("\nInventory: ");

    //List<Item> unequippedInventory = InventoryHelper.GetUnequippedItemsInInventory(unit);
    List<UnitItem> unequippedInventory = unit.UnitItems.Where( ui => ui.Unit == unit && ui.Slot == EquipmentSlot.None).ToList()!;
    if (unequippedInventory.Count() != 0)
    {
        foreach (UnitItem unitItem in unequippedInventory!)
        {
            invTable.AddRow(" - " + unitItem.ToString());
        }
    }
    else
    {
        invTable.AddRow("(No Items)");
    }

    Grid roomTable = new Grid();
    roomTable.AddColumn();
    roomTable.AddRow("Current Room: " + (unit.CurrentRoom == null ? "null" : unit.CurrentRoom.Name));

    Grid abilityTable = new Grid();
    abilityTable.AddColumn();
    abilityTable.AddRow("Abilities: ");

    if (abilities.Any())
    {
        foreach(var ability in abilities)
        {
            abilityTable.AddRow(" - " + ability.Name);
        }
    }
    abilityTable.AddRow("");

    // Creates a display table that contains all the other tables to create a nice little display.
    Table displayTable = new Table();
    displayTable
        .AddColumn(new TableColumn(charTable))
        .AddColumn(new TableColumn(hpTable))
        .AddRow(roomTable, abilityTable)
        .AddRow(invHeader, invTable);

    // Displays the table to the user.
    AnsiConsole.Write(displayTable);
}
```
### Search for a specific Character by name
- Perform a case-insensitive search.
- Display detailed information about the Character, as above.
###### We have the option to search for a character by name
```csharp
private Unit? FindCharacterByName(string name) // Finds and returns a character based on input.
   {
       Unit unit = _unitService.GetAll().Where(c => c.Name.ToUpper() == name.ToUpper()).FirstOrDefault();
       return unit;
   }
```
###### We also have the option to search for a character by using a list.  This method shows the user a list of characters that they can select from.
```csharp
public void FindCharacterByList() // Asks the user for a name and displays a character based on input.
{
    IUnit unit = _ui.PartyUnitSelectionMenu.Display("Select unit to view.", "[[Cancel Character Search]]");

    Console.Clear();

    if (unit != null)
    {
        _characterUI.DisplayCharacterInfo(unit);
    }
    else
    {
        AnsiConsole.MarkupLine($"[White]Character search cancelled.\n[/]");
    }
}
```

### Logging (should already be in place)
- Log all user interactions, such as adding, editing, or displaying data.
###### It's just logs.
```csharp
private readonly static ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .ClearProviders()
        .AddFile("Logs/game-log.txt");
});
```
```json
{
	"ConnectionStrings": {
		"DbConnection": "Data Source=bitsql.wctc.edu;Database=kstephenson1_20023_final_ConsoleGame;MultipleActiveResultSets=True;User ID=**********;Password=*********"
	},
	"Logging": {
		"LogLevel": {
			"Default": "Information",
			"Microsoft": "Warning"
		},
		"Console": {
			"LogLevel": {
				"Default": "Critical"
			}
		},
		"File": {
			"LogLevel": {
				"Default": "Trace"
			},
			"Append": true,
			"FileSizeLimitBytes": 10485760,
			"MaxRollingFiles": 5,
			"MinLevel": "Trace"
		}
	}
}
```


## "C" Level (405/500 points):
### Include all necessary required features.
### Add Abilities to a Character
- Allow users to add Abilities to existing Characters.
- Prompt for related Ability details (for example, Name, Attack Bonus, Defense Bonus, etc).
- Associate the Ability with the Character and save it to the database.
- Output the results of the add to confirm back to the user.
###### The add ability to character shows a list of available abilities able to be added to a character.  Then it shows a list of characters to select from, then adds the ability to that character.
```csharp
public void AddAbilityToCharacter() // Adds an ability to a character.
{
    Type abilityType = _ui.AbilityTypeSelectionMenu.Display("Select an ability to add.", "[[Cancel Ability Selection]]");
    if (abilityType == null) return;
    IUnit unit = _ui.PartyUnitSelectionMenu.Display($"Select unit to add the {abilityType.Name} ability to.", "[[Cancel Ability Selection]]");
    if (unit == null) return;

    Ability ability = (Ability)Activator.CreateInstance(abilityType);

    if (unit is Unit)
    {
        Unit character = (Unit)unit;
        character.Abilities.Add(ability);
        _unitService.Update(character);
        _unitService.Commit();
        AnsiConsole.MarkupLine($"Added ability [#00ffff]{ability.Name}[/] to [#00ffff]{unit.Name}[/]");
    }
}
```
### Display Character Abilities
- For a selected Character, display all their Abilities.
- Include the added properties from their abilities in the output (example, as above, Name, Attack Bonus, Defense Bonus, etc).
###### A really simple method where the user can select a unit and it shows that abilities are known by that unit.
```csharp
public void DisplayAbilitiesForUnit()
{
    IUnit unit = _ui.PartyUnitSelectionMenu.Display("Select a unit to view it's abilities", "[[Go Back]]");
    if (unit == null) return;
    if (unit.Abilities.Any())
    {
        Console.WriteLine($"Abilities usable by {unit.Name}:");
        foreach (var ability in unit.Abilities)
        {
            Console.WriteLine($"{ability.Name} | {ability.Description}");
        }
    }
    else
    {
        Console.WriteLine($"{unit.Name} has no abilities.");
    }
}
```
### Execute an ability during an attack
- When attacking ensure the ability is executed and displays the appropriate output.
###### The abstract class Ability holds information about abilities including the name, description, a which units have this ability.
```csharp
public abstract class Ability : IDatabaseEntity
{
    // Ability is an abstract class that holds basic ability properties and functions.
    public int Id { get; set; }
    public abstract string AbilityType { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual List<Unit> Units { get; }
    protected Ability() { }

    protected Ability(string name, string description)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// CanUseAbility checks to see if the unit can use the ability.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public bool CanUseAbility(Unit unit)
    {
        if (unit.Abilities.Contains(this))
            return true;
        return false;
    }

    /// <summary>
    /// Execute is triggered when the ability is used.
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="target"></param>
    public abstract void Execute(Unit unit, Unit target);
}
```
###### HealAbility is only one of the abilities that can be added to a character in the game.  These ability concrete classes hold specific execution logic for each ability.  For example, the heal ability starts a combat encounter but heals instead of doing damage.  I did this by simply multiplying the combat damage roll by -1 to heal instead of damage.
```csharp
public class HealAbility : Ability
{
    // HealAbility represents the ability to heal.
    public override string AbilityType { get; set; } = "HealAbility";

    public HealAbility() : base()
    {
        Name = "Heal";
        Description = "Heals a friendly unit.";
    }
    public override void Execute(Unit unit, Unit target)
    {
        if(CanUseAbility(unit))
        {
            Encounter encounter = new(unit, target);
            if (encounter.IsCrit())
            {
                Console.WriteLine($"{unit.Name} critically heals {target.Name} for {encounter.Damage} hit points!");
                target.Damage((encounter.Damage) * -1);
            }
            else if (encounter.IsHit())
            {
                Console.WriteLine($"{unit.Name} heals {target.Name} for {encounter.Damage} hit points.");
                target.Damage((encounter.Damage) * -1);
            }
            else
            {
                Console.WriteLine($"{unit.Name}'s misses {target.Name}");
            }
        }
        else
        {
            Console.WriteLine($"{unit} does not have the ability to heal.");
        }
    }
}

```
###### The AbilityCommand is part of the command system that allows for easy execution of commands.
```csharp
public class AbilityCommand : ICommand
{
    // A generic attack command.  It takes in an attacking unit and a target, creates a new encounter object, and
    // calculates whether or not the unit hit/crit and calculates damage.  If the unit cannot attack, a message is provided to the user.
    private readonly IUnit _unit;
    private readonly IUnit _target;
    private readonly Ability _ability;

    public AbilityCommand() { }
    public AbilityCommand(Ability ability, IUnit unit, IUnit target)
    {
        _ability = ability;

        if (unit == null || target == null)
            return;
        _unit = unit;

        _target = target;
    }
    public void Execute()
    {
        if (_unit.Abilities.Contains(_ability))
        {
            Console.WriteLine($"{_unit.Name} uses ability {_ability.Name}.");
            _ability.Execute((Unit)_unit, (Unit)_target);
        }
        else
        {
            Console.WriteLine($"{_unit.Name} does not have ability {_ability.Name}.");
        }
        
    }
}
```
###### The HandleCommand method checks to see what type of command has been input and reacts accordingly.  In this case, AbilityCommand is selected which asks the user to select from a list of abilities a unit has.  If the HealAbility is selected, a list of friendly targets is used, while if an offensive ability is selected you can choose an enemy instead.
```csharp
public bool HandleCommand(ICommand command, IUnit unit)
{
    // If the unit is able to move, the unit moves.
    if (command.GetType() == typeof(MoveCommand))
    {
        unit.Move();
        return true;
    }
    .
    .
    .
    else if (command.GetType() == typeof(AbilityCommand))
    {
        Ability ability = _userInterface.AbilitySelectionMenu.Display(unit, "Select ability to use", "[[Go Back]]");
        IUnit target;
    
        if (ability is HealAbility)
            target = _userInterface.PartyUnitSelectionMenu.Display($"Select target for {ability.Name}.", "[[Go Back]]");
        else
            target = _userInterface.EnemyUnitSelectionMenu.Display($"Select target for {ability.Name}.", "[[Go Back]]");
    
        unit.UseAbility(ability, target);
        return true;
    }
    return false;
}
```

## "B" Level (445/500 points):
### Include all required and "C" level features.
### Add new Room
- Prompt the user to enter a Room name, Description, and other needed properties
- Optionally add a character, player, etc, to that room.
- Save the Room to the database.
- Output the results of the add to confirm back to the user.
###### This method creates a room.  Pretty simple.
```csharp
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
```
###### Also added an EditRoom method where you can edit the name and description of a room.
```csharp
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
```
### Display details of a Room
- Display all associated properties of the room.
- Include a list of any inhabitants in the Room.
- Handle cases where the Room has no Characters gracefully.
###### Just like displayCharacters, the DisplayRooms(List) creates a list of rooms and then displays them.
```csharp
public void DisplayRooms(List<Room> rooms) // Displays the rooms and their info.
{
    // Creates a display table that contains all the other tables to create a nice little display.
    foreach (Room room in rooms)
    {
        DisplayRoom(room);
    }
}
```
###### Just like the DisplayCharacters method once again, spectre.console is used to create a nice little table for each room and display all the information for each room, including the name, description, and a list of units.
```csharp
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
```
### Navigate the Rooms
- Allow the character to navigate through the rooms and display room details upon entering.
  - Room details may include, for example, name, description, inhabitants, special features, etc.
- Note it is not necessary to display a map as provided during the midterm.
###### Navigate is a method that asks the user to select a character to navigate, then runs in a loop and allows the user to navigate the rooms until they are satisfied with the room they are in.  After they decide to stay in a room, the room is updated in the database and then the updated room info is displayed to the user.
```csharp
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
```
###### The RoomNavigationMenu takes in a room and displays a list of adjacent rooms to the user.  When a user selects the room from the interactive menu, it returns the IRoom
```csharp
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
        throw new ArgumentException("Update(item) requires a room.");
    }

    public void Update(IRoom room, string exitMessage)
    {
        _menuItems = new();

        foreach (AdjacentRoom adjacentRoom in room.AdjacentRooms)
        {
            AddMenuItem($"Go {adjacentRoom.Direction.ToString()} to {adjacentRoom.ConnectingRoom.Name}", $"{adjacentRoom.ConnectingRoom.Description}", adjacentRoom.ConnectingRoom);
        }

        AddMenuItem(exitMessage, "", null!);
    }
}
```

## "A" Level (475/500 points):
### Include all required, "C" and "B" level features.
### These features might represent if you were an "admin" character in the game.
  - List characters in the room by selected attribute:
    - Allow users to find the Character in the room matching criteria (e.g. Health, Attack, Name, etc).
###### ListCharacterByAttribute allows the user to select a criteria to filter out the units with.  You select the criteria, then the operand (if needed), and then the value.
```csharp
public void ListCharactersByAttribute()
{
    string attributeType = _ui.AttributeSelectionMenu.Display("Select unit attribute to search by", "[[Cancel Character Search]]");
    if (attributeType == "") return;

    List<Unit> units = new();
    if (attributeType == "name")
    {
        FindCharacterByName();
    }
    else if (attributeType == "class")
    {
        Type classType = _ui.ClassTypeSelectionMenu.Display("Select class to search for", "[[Cancel Character Search]]");
        if (classType == null) return;
        List<Unit> allUnits = _unitService.GetAll().ToList();
        foreach (Unit unit in allUnits)
        {
            if (classType.IsInstanceOfType(unit))
            {
                units.Add(unit);
            }
        }
        if (units.Count == 0)
        {
            AnsiConsole.MarkupLine($"[Red]No characters found with the class {classType.Name}[/]\n");
            return;
        }
        _characterUI.DisplayCharacterInfo(units);
    }
    else
    {
        if (attributeType == "") return;
        string operandType = _ui.OperandSelectionMenu.Display("Select operand to search by", "[[Cancel Character Search]]");
        if (operandType == "") return;

        int compareValue = Input.GetInt($"Search for characters where {attributeType} {operandType} ", 0, "Value must be zero or greater.");

        List<Unit> allUnits = _unitService.GetAll().ToList();
        foreach (Unit unit in allUnits)
        {
            if (DoesAttributeMatchConditions(unit, attributeType, operandType, compareValue))
            {
                units.Add(unit);
            }
        }

        if (units.Count == 0)
        {
            AnsiConsole.MarkupLine($"[Red]No characters found where {attributeType} {operandType} {compareValue}[/]\n");
            return;
        }

        _characterUI.DisplayCharacterInfo(units);
    }
}
```
###### This method takes in a unit, attributetype string, an operand string, and an in to compare the value to.  This method retrieves the value of the attributetype, then proceeds to check if that logic is valid.
```csharp
private bool DoesAttributeMatchConditions(Unit unit, string attributeType, string operand, int compareValue)
{
    int attributeValue = GetAttributeFromUnit(unit, attributeType);
    if (operand == "<")
    {
        return attributeValue < compareValue;
    }
    else if (operand == "<=")
    {
        return attributeValue <= compareValue;
    }
    else if (operand == "==")
    {
        return attributeValue == compareValue;
    }
    else if (operand == ">=")
    {
        return attributeValue >= compareValue;
    }
    else if (operand == ">")
    {
        return attributeValue > compareValue;
    }

    return false;
}
```
###### The attribute is retrieved based on the string entered and returns an int with the value of the specified attribute.
```csharp
private int GetAttributeFromUnit(Unit unit, string attributeType)
{
    int attributeValue = 0;
    switch (attributeType)
    {
        case "name":
            break;
        case "class":
            break;
        case "hp":
            attributeValue = unit.Stat.MaxHitPoints;
            break;
        case "level":
            attributeValue = unit.Level;
            break;
        case "strength":
            attributeValue = unit.Stat.Strength;
            break;
        case "magic":
            attributeValue = unit.Stat.Magic;
            break;
        case "dexterity":
            attributeValue = unit.Stat.Dexterity;
            break;
        case "speed":
            attributeValue = unit.Stat.Speed;
            break;
        case "luck":
            attributeValue = unit.Stat.Luck;
            break;
        case "defense":
            attributeValue = unit.Stat.Defense;
            break;
        case "resistance":
            attributeValue = unit.Stat.Resistance;
            break;
    }
    return attributeValue;
}
```
  - List all Rooms with all characters in those rooms
    - Group Characters by their Room and display them in a formatted list.
###### As shown above, this is similar to DisplayCharacters where it feeds in a list of rooms to the RoomUI where it displays all the room information.
```csharp
public void DisplayRooms()                       //Displays each c's information.
{
    List<Room> rooms = _roomService.GetAll().ToList();

    Console.Clear();

    _roomUi.DisplayRooms(rooms);
}
```
```csharp
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
```
### Find a specific piece of a equipment and list the associated character and location
- Allow a user to specify the name of an item and output the following,
  - Character holding the item
  - Location of the character
###### This method asks the user to input a string value then it checks to see if any items in the database contain that entered string.  if so, it displays all units that has those items.
```csharp
  public void DisplayUnitsWithItem()
  {
      string itemName = Input.GetString("Enter the name of the item to search for: ");
      List<Item> items = _itemService.GetAll().Where(i => i.Name.ToLower().Contains(itemName.ToLower())).ToList();
      List<UnitItem> unitItems = _unitItemService.GetAll().Where(ui => items.Contains(ui.Item)).ToList();

      if (!unitItems.Any())
      {
          AnsiConsole.MarkupLine($"[Red]No items found containing the input \"{itemName}\"[/]\n");
          return;
      }

      List<Unit> units = new();

      foreach (UnitItem item in unitItems)
      {
          if (units.Contains(item.Unit)) continue;
          units.Add(item.Unit);
      }

      if (units.Any())
      {
          Console.WriteLine($"Units with {itemName}:");
          foreach (Unit unit in units)
          {
              _characterUI.DisplayCharacterInfo(unit);
          }
      }
      else
      {
          Console.WriteLine($"No units found with {itemName}.");
      }
  }

```


## "A+" Stretch Level (500/500 points):
### The sky is the limit here! Be creative!
### Include all "C", "B", and "A" level features.
### Stretch Feature: Implement something creative of your own making
- This can be anything including such things as,
  - Interface improvements
  - Database improvements
  - Architectural changes
  - New feature ideas,
    - mini "quest" system
    - enhanced combat system
    - spell casting system
    - item collection system
    - equipment swapping
    - other character types for providing details
    - etc.
###### Here we have a fairly simple class called Menu.  The menu holds a Spectre.Console table object as well as a list of MenuItems.  It includes fnctions to add new menu items, build, and show the table.  Menu is an abstract class that is used to build upon the more complicated menus crated.
```csharp
public abstract class Menu
{
    // The Menus class is an abstract(ish) class to build other menus off of.  The Menus class holds a table which is
    // part of the user interface which is displayed to the user.  The Menu also holds menu items, which can store
    // different types of data.  It can be used by itself if you want a simple message box.

    protected Table _table = new();
    protected List<MenuItem> _menuItems = new();

    public virtual void AddMenuItem(string name) // Adds a new menu item to the menu.
    {
        _menuItems.Add(new MenuItem(_menuItems.Count + 1, name));
    }

    protected virtual void BuildTable() // Builds and stores a custom table for the menu using the menu items stored.
    {
        _table.AddColumn("Header");

        foreach (MenuItem item in _menuItems)
        {
            _table.AddRow(item.Name);
        }
        _table.HideHeaders();
    }

    public virtual void Show() // Shows the menu (Shows the table)
    {
        AnsiConsole.Write(_table);
    }
}
```
###### Menu Item is a simple object that holds an index and a name.
```csharp
public class MenuItem
{
    // MenuItem is an object to be stored in a menu.  It holds an index and a name.
    public int Index { get; protected set; }
    public string Name { get; set; }

    public MenuItem(int index, string name)
    {
        Index = index;
        Name = name;
    }
}
```
###### The InteractiveMenu is a derived abstract class that builds on the basic menu object by adding the functions required to add the little arrow to show your selection, including a new column for the "->", overridden methods for building and showing the table, and a few validation methods for the key press events.  by using the W/Up, S/Down, or E/Enter buttons you can control the menu.  The InteractiveMenu does not use MenuItems, but uses a derived InteractiveMenuItem instead. 
```csharp
public abstract class InteractiveMenu : Menu
{
    // The interactive menu is a menu where you can select the options by moving the arrow selector up and down
    // using the arrow or w/s keys. The MainMenu contains items that have 4 parts, the index, the name, the
    // description, and the action that is completed when that menu item is chosen.
    public InteractiveMenu()
    {

    }

    public InteractiveMenu(int selectedIndex)
    {
        _selectedIndex = selectedIndex;
    }

    protected int _selectedIndex = 0;
    public void AddMenuItem(string name, string desc)
    {
        _menuItems.Add(new InteractiveMenuItem(_menuItems.Count, name, desc));
    }

    public virtual void Display(string errorMessage)
    {
        bool exit = false;
        while (exit != true)
        {
            Console.Clear();
            Update(errorMessage);
            Show();
            ConsoleKey key = ReturnValidKey();
            DoKeyAction(key, out exit);
        }
    }

    protected override void BuildTable()
    {
        _table = new();
        // Creates a table using Spectre.Console and stores that table for later.
        _table.AddColumn("#");
        _table.AddColumn("Selection");
        _table.AddColumn("Description");

        foreach (InteractiveMenuItem item in _menuItems)
        {
            _table.AddRow(GetSelectableArrow(item), item.Name, item.Description);
        }

        _table.HideHeaders();
    }

    protected string GetSelectableArrow(InteractiveMenuItem item)
    {
        if (_selectedIndex == item.Index)
        {
            return "->";
        }
        return " ";
    }

    protected virtual void MenuSelectUp()
    {
        if (_selectedIndex > 0) _selectedIndex--;
    }

    protected virtual void MenuSelectDown()
    {
        if (_selectedIndex < _menuItems.Count - 1) _selectedIndex++;
    }

    protected virtual bool MenuSelectEnter()
    {
        return _selectedIndex == _menuItems.Count - 1;
    }

    protected ConsoleKey ReturnValidKey()
    {
        while (true)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    return ConsoleKey.UpArrow;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    return ConsoleKey.DownArrow;
                case ConsoleKey.E or ConsoleKey.Enter:
                    return ConsoleKey.Enter;
                default:
                    break;
            }
        }
    }

    protected virtual void WaitForEnterPress()
    {
        AnsiConsole.MarkupLine($"Press [green][[ENTER]][/] to continue...");
        while (true)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Enter)
                return;
        }
    }

    protected virtual void DoKeyAction(ConsoleKey key, out bool exit)
    {
        exit = false;
        switch (key)
        {
            case ConsoleKey.UpArrow:
                MenuSelectUp();
                break;
            case ConsoleKey.DownArrow:
                MenuSelectDown();
                break;
            case ConsoleKey.Enter:
                exit = MenuSelectEnter();
                WaitForEnterPress();
                break;
        }
    }
    protected virtual void Update(string exitMessage)
    {
        BuildTable();
    }
    protected int GetSelectedIndex() => _selectedIndex;
}
```
###### The InteractiveMenuItem object builds off the MenuItem but adds an index which is used to keep track of where the arrow is.
```csharp
public class InteractiveMenuItem : MenuItem
{
    // The MainMenuItem is used to store information about the selection in the main menu.  This stores the index and name (from the base),
    // description, and an acion in a handy object that can be referenced easily later.
    public string Description { get; set; }

    public InteractiveMenuItem(int index, string name, string desc) : base(index, name)
    {
        Index = index;
        Name = name;
        Description = desc;
    }
}
```
###### The MainMenu is another abstract menu class that derives from InteractiveMenu.  It allows the capability to use the arrow keys and enter to control the menus, but adds the added functionality of using an InteractiveSelectionMenuItem<Action>, which in addition to holding an index, name, and description also holds an action.  These actions are used in the main menu
```csharp
public abstract class MainMenu : InteractiveMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    protected bool _exit = false;

    public MainMenu()
    {
        
    }

    public virtual void Display() => Display("[[Go Back]]");

    public override void Display(string exitMessage)
    {
        _exit = false;
        while (_exit != true)
        {
            Console.Clear();
            Update(exitMessage);
            Show();
            ConsoleKey key = ReturnValidKey();
            DoKeyAction(key, out _exit);
            if (_exit) break;
        }
    }

    protected void AddMenuItem(string name, string desc, Action action)
    {
        _menuItems.Add(new InteractiveSelectionMenuItem<Action>(_menuItems.Count, name, desc, action));
    }

    protected void Action(int selection)
    {
        // The Action method takes in a selecion from the main menu, then triggers the action associated with that menu item.
        List<InteractiveSelectionMenuItem<Action>> menuItems = new();

        foreach (MenuItem item in _menuItems) // Casts each of the MenuItems into MainMenuItems so the actions can work.
        {
            menuItems.Add((InteractiveSelectionMenuItem<Action>)item);
        }

        menuItems[selection].Selection(); // Runs the action selected.
    }

    protected override void Update(string exitMessage)
    {
        /* Example code block of a main menu update override
         * 
         * _menuItems = new();
         * AddMenuItem("Option 1", "Option 1 Example", Class.Action1);
         * AddMenuItem("Option 2", "Option 2 Example", Class.Action2);
         * AddMenuItem(exitMessage, "", End);
         * BuildTable();
         *
         */
    }

    protected override bool MenuSelectEnter()
    {
        Action(_selectedIndex);
        return _selectedIndex == _menuItems.Count - 1 ? true : false;
    }

    protected virtual void End()
    {
        _exit = true;
    }

    protected override void WaitForEnterPress()
    {
        if (_exit) return;
        base.WaitForEnterPress();
    }
}
```
###### The InteractiveSelectionMenuItem is almost the same as InteractiveMenuItem, except that it can hold a T Type object.  In the main menu, this is used to hold an action to store menu items that, when selected, trigger an action.  It is used in a lot more menus throughout the game such as selecting units, items, rooms, abilities, and more.  When these menu options are selected, they return the object attached to the menu item.
```csharp
public class InteractiveSelectionMenuItem<T> : InteractiveMenuItem
{
    // The InteractiveReturnMenuItem is used to store information about the selection in the main menu.  This stores the index and name (from the base),
    // description, and a generic type.
    public T Selection { get; set; }

    public InteractiveSelectionMenuItem(int index, string name, string desc, T selection) : base(index, name, desc)
    {
        Index = index;
        Name = name;
        Description = desc;
        Selection = selection;
    }
}
```
###### This is the MainMenuRoot.  This is the main part of the main menu that implements the MainMenu object.  This is a simple class that holds a custom collection of menu items specifically for the MainMenu.  There are also main menu subfolders that contain more main menus to declutter the main menu.  For example, the character related menu items are in MainMenuCharacter, and there are also main menu submenus for abilities, inventory, and rooms which are listed below.
```csharp
public class MainMenuRoot : MainMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private readonly CharacterUtilities _characterUtilities;
    private readonly MainMenuAbilities _mainMenuAbilities;
    private readonly MainMenuInventory _mainMenuInventory;
    private readonly MainMenuCharacters _mainMenuCharacters;
    private readonly MainMenuRooms _mainMenuRooms;
    public MainMenuRoot(CharacterUtilities characterUtilities, MainMenuAbilities mainMenuAbilities, MainMenuCharacters mainMenuCharacters, MainMenuInventory mainMenuInventory, MainMenuRooms mainMenuRooms)
    {
        _characterUtilities = characterUtilities;
        _mainMenuAbilities = mainMenuAbilities;
        _mainMenuCharacters = mainMenuCharacters;
        _mainMenuInventory = mainMenuInventory;
        _mainMenuRooms = mainMenuRooms;
    }

    protected override void Update(string exitMessage)
    {
        _menuItems = new();
        AddMenuItem("Display Characters", "Displays all characters and their stats.", _characterUtilities.DisplayCharacters);
        AddMenuItem("Character Options", "Shows options for character management", _mainMenuCharacters.Display);
        AddMenuItem("Ability Options", "Shows options for ability management", _mainMenuAbilities.Display);
        AddMenuItem("Dungeon/Room Options", "Shows options for dungeons and/or room management", _mainMenuRooms.Display);
        AddMenuItem("Inventory Options", "Shows options for inventory management", _mainMenuInventory.Display);
        AddMenuItem(exitMessage, "", End);
        BuildTable();
    }
}
```
```csharp
public class MainMenuAbilities : MainMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private CharacterUtilities _characterUtilities;

    public MainMenuAbilities(CharacterUtilities characterUtilities)
    {
        _characterUtilities = characterUtilities;
    }

    protected override void Update(string exitMessage)
    {
        _menuItems = new();
        AddMenuItem("View Abilities for Character", "Shows performable abilities by character.", _characterUtilities.DisplayAbilitiesForUnit);
        AddMenuItem("Add Ability to Character", "Adds an ability to a character", _characterUtilities.AddAbilityToCharacter);
        AddMenuItem(exitMessage, "", End);
        BuildTable();
    }
}
```
```csharp
public class MainMenuCharacters : MainMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private CharacterUtilities _characterUtilities;

    public MainMenuCharacters(CharacterUtilities characterUtilities)
    {
        _characterUtilities = characterUtilities;
    }
    
    protected override void Update(string exitMessage)
    {
        _menuItems = new();
        AddMenuItem("New Character", "Creates a new character.", _characterUtilities.NewCharacter);
        AddMenuItem("Edit Character", "Select a unit and change it's properties.", _characterUtilities.EditCharacter);
        AddMenuItem("Find Character by Name", "Finds an existing character by name.", _characterUtilities.FindCharacterByName);
        AddMenuItem("Find Character by List", "Shows a list of units to select from", _characterUtilities.FindCharacterByList);
        AddMenuItem("Find Character by Attribute", "Search a character by name, class, stats, etc.", _characterUtilities.ListCharactersByAttribute);
        AddMenuItem("Edit Level of Character by Search", "Level up/down a selected character.", _characterUtilities.LevelUp);
        AddMenuItem("Edit Level of Character by List", "Level up/down a selected character.", _characterUtilities.LevelUpByList);
        AddMenuItem(exitMessage, "", End);
        BuildTable();
    }
}
```
```csharp
public class MainMenuInventory : MainMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private CharacterUtilities _characterUtilities;
    private ItemService _itemService;

    public MainMenuInventory(CharacterUtilities characterUtilities, ItemService itemService)
    {
        _characterUtilities = characterUtilities;
        _itemService = itemService;
    }

    protected override void Update(string exitMessage)
    {
        _menuItems = new();
        AddMenuItem("Search for Item by Name", "Finds a list of items based on search.", _itemService.SearchItemByName);
        AddMenuItem("List Items by Type", "Lists all items sorted by type.", _itemService.ListItemsByType);
        AddMenuItem("List Items Sorted by Name", "Lists all items in alphabetical order.", _itemService.ListItemsByName);
        AddMenuItem("List Weapons Sorted by Might", "Lists all weapons from weakest to strongest.", _itemService.ListItemsByMight);
        AddMenuItem("List Armor Sorted by Defense", "Lists all armors from weakest to strongest defense.", _itemService.ListItemsByDefense);
        AddMenuItem("List Armor Sorted by Resistance", "Lists all armors from weakest to stringest resistance.", _itemService.ListItemsByResistance);
        AddMenuItem("List Armor Sorted by Durability", "Lists all equippable items from least to most durability.", _itemService.ListItemsByDurability);
        AddMenuItem("Find units with Item", "Enter item name and search for all characters with that item.", _characterUtilities.DisplayUnitsWithItem);
        AddMenuItem(exitMessage, "", End);
        BuildTable();
    }
}
```
```csharp
public class MainMenuRooms : MainMenu
{
    // The MainMenu contains items that have 4 parts, the index, the name, the description, and the action that
    // is completed when that menu item is chosen.  It loops until the menu is exited.
    private readonly RoomUtilities _roomUtilities;

    public MainMenuRooms(RoomUtilities roomUtilities)
    {
        _roomUtilities = roomUtilities;
    }

    protected override void Update(string exitMessage)
    {
        _menuItems = new();
        AddMenuItem("Display Rooms", "Displays all rooms and their descriptions.", _roomUtilities.DisplayRooms);
        AddMenuItem("New Room", "Creates a new room.", _roomUtilities.CreateRoom);
        AddMenuItem("Edit Room", "Edits an existing room.", _roomUtilities.EditRoom);
        AddMenuItem("Find Room by Name", "Finds a room based on search.", _roomUtilities.FindRoomByName);
        AddMenuItem("Find Room by List", "Finds a room chosen by a list.", _roomUtilities.FindRoomByList);
        AddMenuItem("Navigate rooms", "Select a character and navigate the rooms", _roomUtilities.Navigate);
        AddMenuItem(exitMessage, "", End);
        BuildTable();
    }
}
```
  
## Submission Requirements:
### Submit the following:
- A video demonstrating the full functionality of your application (approximately 5 minutes). I recommend using Canvas Studio.
- A link to your GitHub repository and your database connection string.
- A README file,
  - quickly describe the features you added at each grade level and grading level you attempted to achieve
  - include any final comments on the class in the provided README file
- Use in-class examples and provided resources to complete the assignment.
- Handle all user errors gracefully (e.g., invalid input, database issues) and log all errors.
- Most of all.. Have fun!
