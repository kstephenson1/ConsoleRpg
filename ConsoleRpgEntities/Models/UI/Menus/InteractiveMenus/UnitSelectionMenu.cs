using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Services.Repositories;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class UnitSelectionMenu : InteractiveSelectionMenu<IUnit>
{
    // UnitSelectionMenu is used to select a unit from a list of units.  It takes in a prompt and an _exit message,
    // and displays a list of units to select from. It returns the selected unit or null if the user exits the menu.

    private readonly UnitService _unitService;
    private readonly StatService _statService;

    public UnitSelectionMenu(StatService statService, UnitService unitService)
    {
        _unitService = unitService;
        _statService = statService;
    }

    public override IUnit Display(string prompt, string exitMessage   )
    {
        IUnit selection = default!;
        bool exit = false;
        while (exit != true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Update(exitMessage);
            BuildTable(exitMessage);
            Show();
            ConsoleKey key = ReturnValidKey();
            selection = DoKeyActionReturnUnit(key, out exit);
        }
        return selection;
    }

    public override void Update(string exitMessage)
    {
        _menuItems = new();

        IEnumerable<Unit> units = _unitService.GetAll();
        List<Unit> characters = new();
        List<Unit> monsters = new();
        foreach(Unit unit in units)
        {
            if (unit.UnitType.Contains("Enemy"))
                {
                monsters.Add(unit);
            }
            else
            {
                characters.Add(unit);
            }
        }

        // Adds all the characters to the unit list using green letters.
        foreach (IUnit unit in characters)
        {
            // Strikethrough and dim the unit info if the unit is not alive.
            if (unit.Stat.HitPoints <= 0)
            {
                AddMenuItem($"[green][dim][strikethrough]{unit.Name} Level {unit.Level} {unit.Class}[/][/][/]", $" {unit.GetHealthBar()}", unit);
            }
            else
            {
                AddMenuItem($"[green][bold]{unit.Name}[/][/] Level {unit.Level} {unit.Class}", $" {unit.GetHealthBar()}", unit);
            }
        }
        // Adds all the monsters to the unit list using red letters.
        foreach (IUnit unit in monsters)
        {
            if (unit.Stat.HitPoints <= 0)
            {
                // Strikethrough and dim the unit info if the unit is not alive.
                AddMenuItem($"[red][dim][strikethrough]{unit.Name} Level {unit.Level} {unit.Class}[/][/][/]", $" {unit.GetHealthBar()}", unit);
            }
            else
            {
                AddMenuItem($"[red][bold]{unit.Name}[/][/] Level {unit.Level} {unit.Class}", $" {unit.GetHealthBar()}", unit);
            }
        }
        AddMenuItem(exitMessage, $"", null!);
    }
}

