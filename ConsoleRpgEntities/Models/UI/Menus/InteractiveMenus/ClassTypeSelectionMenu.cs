using System.Reflection;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class ClassTypeSelectionMenu : InteractiveSelectionMenu<Type>
{
    // ClassTypeSelectionMenu is used to select a unit class from a list of unit classes.  It's used for character creation.

    public ClassTypeSelectionMenu()
    {

    }

    public override Type Display(string prompt, string exitMessage)
    {
        Type selection = default!;
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

        string characterNamespace = "ConsoleRpgEntities.Models.Units.Characters";
        IEnumerable<Type> unitTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsClass && t.Namespace == characterNamespace
                select t;

        foreach (Type unitType in unitTypes)
        {
            AddMenuItem($"{unitType.Name}", $"", unitType);
        }

        AddMenuItem(exitMessage, $"", null!);
    }
}

