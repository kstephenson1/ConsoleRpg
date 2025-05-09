namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class EditCharacterMenu : InteractiveSelectionMenu<Action>
{
    // LevelUpCharacterMenu is used to level up or down a unit.  It allows the user to select -1 or 1 to decrease or increase
    // the unit's level. Returns 0 if the user selects the _exit option.


    public EditCharacterMenu()
    {

    }

    public override Action Display(string prompt, string exitMessage)
    {
        Action selection = default!;
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

        AddMenuItem($"Edit Name", $"Changes the name of this unit.", null);
        AddMenuItem($"+1", $"Increases level by 1", null);

        AddMenuItem(exitMessage, $"", null);
    }
}

