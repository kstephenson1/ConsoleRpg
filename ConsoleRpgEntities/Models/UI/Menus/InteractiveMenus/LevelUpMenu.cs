namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class LevelUpMenu : InteractiveSelectionMenu<int>
{
    // LevelUpMenu is used to level up or down a unit.  It allows the user to select -1 or 1 to decrease or increase
    // the unit's level. Returns 0 if the user selects the _exit option.


    public LevelUpMenu()
    {

    }

    public override int Display(string prompt, string exitMessage)
    {
        int selection = default!;
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

        AddMenuItem($"-1", $"Decreases level by 1", -1);
        AddMenuItem($"+1", $"Increases level by 1", 1);

        AddMenuItem(exitMessage, $"", 0);
    }
}

