using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Commands.ItemCommands;
using ConsoleRpgEntities.Models.Commands.UnitCommands;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;
using ConsoleRpgEntities.Models.Interfaces.InventoryBehaviors;
using ConsoleRpgEntities.Models.Interfaces.UnitBehaviors;
using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Commands.AbilityCommands;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class CommandMenu : InteractiveSelectionMenu<ICommand>
{
    // CommandMenu is used to display a menu of commands that can be executed by a unit. It takes in a unit and a prompt,
    // and displays the menu of commands. It also takes in an _exit message, which is displayed at the bottom of the menu.
    // The user can select a command by pressing the corresponding key, and the command is executed. If the user presses
    // the _exit key, the menu is exited.

    private readonly GameContext _db;

    public CommandMenu(GameContext context)
    {
        _db = context;
    }

    public override void Display(string errorMessage)
    {
        throw new ArgumentException("CommandMenu(unit, prompt) requires a unit.");
    }

    public ICommand Display(IUnit unit, string prompt, string exitMessage)
    {
        ICommand selection = default!;
        bool exit = false;
        while (exit != true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Update(unit, exitMessage);
            BuildTable(exitMessage);
            Show();
            ConsoleKey key = ReturnValidKey();
            selection = DoKeyActionReturnUnit(key, out exit);
        }
        return selection;
    }

    public override void Update(string exitMessage)
    {
        throw new ArgumentException("Update(unit) requires a unit.");
    }

    public void Update(IUnit unit, string exitMessage)
    {
        _menuItems = new();

        AddMenuItem("Move", "Moves the unit.", new MoveCommand());

        if (unit is IAttack && InventoryHelper.GetEquippedWeapon((Unit)unit) != null)
            AddMenuItem("Attack", "Attacks a target unit.", new AttackCommand());

        if(unit.Abilities.Any())
        {
            AddMenuItem("Use Ability", "Use an ability that this character can use", new AbilityCommand());
        }

        if (unit is IHaveInventory)
        {
            if (unit.UnitItems!.Count != 0)
                AddMenuItem("Items", "Uses an item in this unit's inventory.", new UseItemCommand());
            else
                AddMenuItem("[dim]Items[/]", "[dim]Uses an item in this unit's inventory.[/]", new UseItemCommand());
        }

        if (unit is ICastable)
            AddMenuItem("Cast", "Casts a spell.", new CastCommand());

        AddMenuItem(exitMessage, "", null!);
    }
}

