namespace ConsoleRpgEntities.Services;

using ConsoleRpgEntities.Config;
using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.UI.Character;
using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Services.DataHelpers;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Rooms;
using Spectre.Console;
using ConsoleRpgEntities.Services.Repositories;
using ConsoleRpgEntities.Models.Items;
using ConsoleRpgEntities.Models.Abilities;

public class CharacterUtilities
{
    private readonly AbilitySelectionMenu _abilitySelectionMenu;
    private readonly CharacterUI _characterUI;
    private readonly LevelUpCharacterMenu _levelUpMenu;
    private readonly UnitService _unitService;
    private readonly StatFactory _statFactory;
    private readonly StatService _statService;
    private readonly RoomMenu _roomMenu;
    private readonly UnitClassSelectionMenu _unitClassMenu;
    private readonly PartyUnitSelectionMenu _unitSelectionMenu;
    // CharacterFunctions class contains fuctions that manipulate characters based on user input.

    public CharacterUtilities(AbilitySelectionMenu abilitySelectionMenu, CharacterUI characterUI, UnitClassSelectionMenu unitClassMenu, LevelUpCharacterMenu levelUpMenu, UnitService unitService, StatService statService, RoomMenu roomMenu, StatFactory statFactory, PartyUnitSelectionMenu unitSelectionMenu)
    {
        _abilitySelectionMenu = abilitySelectionMenu;
        _characterUI = characterUI;
        _levelUpMenu = levelUpMenu;
        _unitService = unitService;
        _statFactory = statFactory;
        _statService = statService;
        _roomMenu = roomMenu;
        _unitClassMenu = unitClassMenu;
        _unitSelectionMenu = unitSelectionMenu;
    }
    public void NewCharacter() // Creates a new character.  Asks for name, class, level, hitpoints, and items.
    {
        string name = Input.GetString("Enter your character's name: ");
        Type characterClass = _unitClassMenu.Display($"Please select a class for {name}", "[[Cancel Character Creation]]");
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


        IRoom room = _roomMenu.Display($"Select room for {character.Name}","[[No Room]]");
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

    public void EditCharacter() // Asks the user for a name and displays a character based on input.
    {
        Unit character = ReturnCharacterByList("Select unit to edit.");
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

            _unitService.Update(character);
            _unitService.Commit();
        }
    }

    public void FindCharacterByName() // Asks the user for a name and displays a character based on input.
    {
        string characterName = Input.GetString("What is the name of the character you would like to search for? ");
        Unit character = FindCharacterByName(characterName)!;

        Console.Clear();

        if (character != null)
        {
            _characterUI.DisplayCharacterInfo(character);
        }
        else
        {
            AnsiConsole.MarkupLine($"[Red]No characters found with the name {characterName}\n[/]");
        }
    }

    public void FindCharacterByList() // Asks the user for a name and displays a character based on input.
    {
        IUnit unit = _unitSelectionMenu.Display("Select unit to view.", "[[Cancel Character Search]]");

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

    private Unit? FindCharacterByName(string name) // Finds and returns a character based on input.
    {
        Unit unit = _unitService.GetAll().Where(c => c.Name.ToUpper() == name.ToUpper()).FirstOrDefault();
        return unit;
    }

    public Unit ReturnCharacterByList(string prompt) // Asks the user for a name and displays a character based on input.
    {
        IUnit unit = _unitSelectionMenu.Display(prompt, "[[Cancel Character Search]]");

        return unit as Unit;
    }

    public void LevelUp() //Asks the user for a character to level up, then displays that character.
    {
        string characterName = Input.GetString("What is the name of the character that you would like to level up? ");
        Unit unit = FindCharacterByName(characterName)!;
        Console.Clear();

        if (unit != null)
        {
            int levelModifier = _levelUpMenu.Display($"Choose how to change the level for {unit.Name}", "Go Back");
            _unitService.Update(unit);
            switch (levelModifier)
            {
                case -1:
                    if (unit.Level > 1)
                    {
                        unit.Level += levelModifier;
                        AnsiConsole.MarkupLine($"[Yellow]Yikes! {unit.Name} has been demoted to level {unit.Level}[/]\n");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[Red]{unit.Name} is level one and cannot go down another level![/]\n");
                    }
                    break;
                case 1:
                    if (unit.Level < Config.CHARACTER_LEVEL_MAX)
                    {
                        unit.Level += levelModifier;
                        AnsiConsole.MarkupLine($"[Green]Congratulations! {unit.Name} has reached level {unit.Level}[/]\n");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[Red]{unit.Name} is already max level! ({Config.CHARACTER_LEVEL_MAX})[/]\n");
                    }
                    break;
                default:
                    AnsiConsole.MarkupLine($"[White]{unit.Name} remains the same level[/]\n");
                    break;
            }
            _characterUI.DisplayCharacterInfo(unit);
            _unitService.Commit();
        }
        else
        {
            AnsiConsole.MarkupLine($"[Red]No characters found with the name {characterName}[/]\n");
        }
    }

    public void LevelUpByList() //Asks the user for a character to level up, then displays that character.
    {
        IUnit unit = _unitSelectionMenu.Display("Select unit to view.", "[[Cancel Level Up/Down]]");
        Console.Clear();

        if (unit != null)
        {
            int levelModifier = _levelUpMenu.Display($"Choose how to change the level for {unit.Name}", "Go Back");
            _unitService.Update(unit as Unit);
            switch (levelModifier)
            {
                case -1:
                    if (unit.Level > 1)
                    {
                        unit.Level += levelModifier;
                        AnsiConsole.MarkupLine($"[Yellow]Yikes! {unit.Name} has been demoted to level {unit.Level}[/]\n");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[Red]{unit.Name} is level one and cannot go down another level![/]\n");
                    }
                    break;
                case 1:
                    if (unit.Level < Config.CHARACTER_LEVEL_MAX)
                    {
                        unit.Level += levelModifier;
                        AnsiConsole.MarkupLine($"[Green]Congratulations! {unit.Name} has reached level {unit.Level}[/]\n");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[Red]{unit.Name} is already max level! ({Config.CHARACTER_LEVEL_MAX})[/]\n");
                    }
                    break;
                default:
                    AnsiConsole.MarkupLine($"[White]{unit.Name} remains the same level[/]\n");
                    break;
            }
            _characterUI.DisplayCharacterInfo(unit);
            _unitService.Commit();
        }
        else
        {
            AnsiConsole.MarkupLine($"[White]Level up cancelled.[/]\n");
        }
    }

    public void DisplayCharacters()                       //Displays each c's information.
    {
        Console.Clear();
        List<Unit> units = _unitService.GetAll().Where(u => !u.UnitType.Contains("Enemy")).ToList();

        _characterUI.DisplayCharacterInfo(units);
    }

    public void AddAbilityToCharacter() // Adds an ability to a character.
    {
        Type abilityType = _abilitySelectionMenu.Display("Select an ability to add.", "[[Cancel Ability Selection]]");
        if (abilityType == null) return;
        IUnit unit = _unitSelectionMenu.Display($"Select unit to add the {abilityType.Name} ability to.", "[[Cancel Ability Selection]]");
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
}
