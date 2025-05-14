namespace ConsoleRpgEntities.Services;

using ConsoleRpgEntities.Config;
using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.UI.Character;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Services.DataHelpers;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Rooms;
using Spectre.Console;
using ConsoleRpgEntities.Services.Repositories;
using ConsoleRpgEntities.Models.Items;
using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.UI;
using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class CharacterUtilities
{
    private readonly CharacterUI _characterUI;
    private readonly UnitService _unitService;
    private readonly UnitItemService _unitItemService;
    private readonly ItemService _itemService;
    private readonly StatFactory _statFactory;
    private readonly StatService _statService;
    private readonly UserInterface _ui;
    // CharacterFunctions class contains fuctions that manipulate characters based on user input.

    public CharacterUtilities(CharacterUI characterUI, UnitService unitService, UnitItemService unitItemService, ItemService itemService, StatService statService, StatFactory statFactory, UserInterface userInterface)
    {
        _characterUI = characterUI;
        _unitService = unitService;
        _unitItemService = unitItemService;
        _itemService = itemService;
        _statFactory = statFactory;
        _statService = statService;
        _ui = userInterface;
    }
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

    private Unit? FindCharacterByName(string name) // Finds and returns a character based on input.
    {
        Unit unit = _unitService.GetAll().Where(c => c.Name.ToUpper() == name.ToUpper()).FirstOrDefault();
        return unit;
    }

    public Unit ReturnCharacterByList(string prompt) // Asks the user for a name and displays a character based on input.
    {
        IUnit unit = _ui.PartyUnitSelectionMenu.Display(prompt, "[[Cancel Character Search]]");

        return unit as Unit;
    }

    public void LevelUp() //Asks the user for a character to level up, then displays that character.
    {
        string characterName = Input.GetString("What is the name of the character that you would like to level up? ");
        Unit unit = FindCharacterByName(characterName)!;
        Console.Clear();

        if (unit != null)
        {
            int levelModifier = _ui.LevelUpCharacterMenu.Display($"Choose how to change the level for {unit.Name}", "Go Back");
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
        IUnit unit = _ui.PartyUnitSelectionMenu.Display("Select unit to view.", "[[Cancel Level Up/Down]]");
        Console.Clear();

        if (unit != null)
        {
            int levelModifier = _ui.LevelUpCharacterMenu.Display($"Choose how to change the level for {unit.Name}", "Go Back");
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
    /*
     * 
     *      AddMenuItem($"Name", $"Search by name.", "name");
            AddMenuItem($"Class", $"Search by unit class.", "class");
            AddMenuItem($"Hit Points", $"Search by hit points value.", "hp");
            AddMenuItem($"Level", $"Search by level.", "level");
            AddMenuItem($"Strength", $"Search by strength stat.", "strength");
            AddMenuItem($"Magic", $"Search by magic stat.", "magic");
            AddMenuItem($"Dexterity", $"Search by dexterity stat.", "dexterity");
            AddMenuItem($"Speed", $"Search by speed stat.", "speed");
            AddMenuItem($"Luck", $"Search by luck stat.", "luck");
            AddMenuItem($"Defense", $"Search by defense stat.", "defense");
            AddMenuItem($"Resistance", $"Search by resistance stat.", "resistance");
     * */
}
