using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Interfaces;

namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;

public class StatSelectionMenu : InteractiveSelectionMenu<string>
{
    // LevelUpMenu is used to level up or down a unit.  It allows the user to select -1 or 1 to decrease or increase
    // the unit's level. Returns 0 if the user selects the _exit option.


    public StatSelectionMenu()
    {

    }
    public override void Display(string errorMessage)
    {
        throw new ArgumentException("StatSelectionMenu requires a stat.");
    }

    public string Display(Stat stat, string prompt, string exitMessage)
    {
        string selection = default!;
        bool exit = false;
        while (exit != true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Update(stat, exitMessage);
            BuildTable(exitMessage);
            Show();
            ConsoleKey key = ReturnValidKey();
            selection = DoKeyActionReturnUnit(key, out exit);
        }
        return selection;
    }

    public override void Update(string exitMessage)
    {
        throw new ArgumentException("Update(StatSelectionMenu) requires a stat.");
    }

    public void Update(Stat stat, string exitMessage)
    {
        _menuItems = new();

        AddMenuItem($"HP: {stat.MaxHitPoints} +1-3", $"Hit points are your unit's health.  If it reaches zero, bad things happen.", "MHP");
        AddMenuItem($"CON: {stat.Constitution} +1", $"Constitution allows the unit to equip heavier weapons with less penalties.", "CON");
        AddMenuItem($"STR: {stat.Strength} +1", $"Strength increases damage done by physicical weapons like swords and bows.", "STR");
        AddMenuItem($"MAG: {stat.Magic} +1", $"Magic increases damage done by magical weapons like fire and smite.", "MAG");
        AddMenuItem($"DEX: {stat.Dexterity} +1", $"Dexterity increases the hit chance of your attacks.", "DEX");
        AddMenuItem($"SPD: {stat.Speed} +1", $"Speed reduces the hit chance for enenmies to hit you with attacks.", "SPD");
        AddMenuItem($"LCK: {stat.Luck} +1", $"Luck slightly increses your hit and crit chance and slightly reduces the odds you are crit.", "LCK");
        AddMenuItem($"DEF: {stat.Defense} +1", $"Defense reduces the damage taken by physical weapons like swords and bows.", "DEF");
        AddMenuItem($"RES: {stat.Resistance} +1", $"Resistance reduces the damage taken by magical weapons like fire and smite.", "RES");

        AddMenuItem(exitMessage, $"Ends the level up process. All unspent points will be lost.", null);    }
}

