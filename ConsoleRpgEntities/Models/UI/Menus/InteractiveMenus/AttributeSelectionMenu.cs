namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class AttributeSelectionMenu : InteractiveSelectionMenu<string>
{
    // LevelUpCharacterMenu is used to level up or down a unit.  It allows the user to select -1 or 1 to decrease or increase
    // the unit's level. Returns 0 if the user selects the _exit option.


    public AttributeSelectionMenu()
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

        AddMenuItem($"Name", $"Search by name.", "name");
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
        AddMenuItem(exitMessage, $"", "");
    }
}

