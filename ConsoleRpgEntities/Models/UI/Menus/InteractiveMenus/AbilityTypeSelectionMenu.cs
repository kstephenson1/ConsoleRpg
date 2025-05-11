using System.Reflection;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class AbilityTypeSelectionMenu : InteractiveSelectionMenu<Type>
{
    // AbilityTypeSelectionMenu is used to select an ability from a list of abilities.  It's used for character creation.

    public AbilityTypeSelectionMenu()
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

        string characterNamespace = "ConsoleRpgEntities.Models.Abilities.UnitAbilities";
        IEnumerable<Type> abilityTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsClass && t.Namespace == characterNamespace
                select t;

        foreach (Type abilityType in abilityTypes)
        {
            
            AddMenuItem($"{abilityType.Name}", $"", abilityType);
        }

        AddMenuItem(exitMessage, $"", null!);
    }
}

