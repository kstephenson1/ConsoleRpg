namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class OperandSelectionMenu : InteractiveSelectionMenu<string>
{
    // LevelUpCharacterMenu is used to level up or down a unit.  It allows the user to select -1 or 1 to decrease or increase
    // the unit's level. Returns 0 if the user selects the _exit option.


    public OperandSelectionMenu()
    {

    }

    public override string Display(string prompt, string exitMessage)
    {
        string selection = default!;
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

        AddMenuItem($"<", $"Less than", "<");
        AddMenuItem($"<=", $"Less than or equal to", "<=");
        AddMenuItem($"=", $"Equal to", "==");
        AddMenuItem($">=", $"Greater than or equal to", ">=");
        AddMenuItem($">", $"Greater than", ">");

        AddMenuItem(exitMessage, $"", "");
    }
}

