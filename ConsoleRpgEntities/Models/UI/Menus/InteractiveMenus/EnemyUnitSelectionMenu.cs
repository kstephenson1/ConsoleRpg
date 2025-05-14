using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Services.Repositories;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class EnemyUnitSelectionMenu : InteractiveSelectionMenu<IUnit>
{
    // PartyUnitSelectionMenu is used to select a unit from a list of units.  It takes in a prompt and an _exit message,
    // and displays a list of units to select from. It returns the selected unit or null if the user exits the menu.

    private readonly UnitService _unitService;

    public EnemyUnitSelectionMenu(UnitService unitService)
    {
        _unitService = unitService;
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
            BuildTable();
            Show();
            ConsoleKey key = ReturnValidKey();
            selection = DoKeyActionReturnUnit(key, out exit);
        }
        return selection;
    }

    protected override void Update(string exitMessage)
    {
        _menuItems = new();

        IEnumerable<Unit> units = _unitService.GetAll();
        List<Unit> monsters = new();
        foreach(Unit unit in units)
        {
            if (unit.UnitType.Contains("Enemy"))
                {
                monsters.Add(unit);
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

