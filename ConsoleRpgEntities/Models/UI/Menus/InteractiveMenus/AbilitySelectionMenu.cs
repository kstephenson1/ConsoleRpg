using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Interfaces;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class AbilitySelectionMenu : InteractiveSelectionMenu<Ability>
{
    // ItemCommandMenu is used to display a menu of commands that can be performed on an item.  It takes in a unit and an
    // item, and displays a menu of commands that can be performed on the item, then returns the command that was
    // selected or returns null if the user exits the menu.

    public override void Display(string errorMessage)
    {
        throw new ArgumentException("CommandMenu(unit, prompt) requires a unit.");
    }

    public Ability Display(IUnit unit, string prompt, string exitMessage)
    {
        Ability selection = default!;
        bool exit = false;
        while (exit != true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Update(unit, exitMessage);
            BuildTable();
            Show();
            ConsoleKey key = ReturnValidKey();
            selection = DoKeyActionReturnUnit(key, out exit);
        }
        return selection;
    }

    protected override void Update(string exitMessage)
    {
        throw new ArgumentException("Update(item) requires an item.");
    }

    public void Update(IUnit unit, string exitMessage)
    {
        _menuItems = new();

        foreach (Ability ability in unit.Abilities)
        {
            AddMenuItem(ability.Name, ability.Description, ability);
        }

        AddMenuItem(exitMessage, "", null!);
    }
}

