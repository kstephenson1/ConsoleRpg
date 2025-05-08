using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;
using ConsoleRpgEntities.Services.Repositories;
using System.ComponentModel;

namespace ConsoleRpgEntities.Services;

public class StatFactory
{
    private readonly StatSelectionMenu _statSelectionMenu;
    private readonly StatService _statService;
    const int MAX_HIT_POINTS = 60;
    const int MAX_STAT_POINTS = 20;

    private Random Random { get; } = new Random();
    public StatFactory(StatSelectionMenu statSelectionMenu, StatService statService)
    {
        _statSelectionMenu = statSelectionMenu;
        _statService = statService;
    }
    public Stat CreateStat(IUnit unit)
    {
        int statsRemaining = 0;
        int levelsRemaining = unit.Level; 

        Stat stat = GetStartingStatsForClass(unit.Class);

        while (levelsRemaining > 0)
        {
            statsRemaining = Random.Next(1, 7); // 1-6 stats to assign per level
            while (statsRemaining > 0)
            {
                string statSelection = _statSelectionMenu.Display(stat, $"Congratulations! {unit.Name} is level {unit.Level - levelsRemaining + 1} (Increase up to {statsRemaining} stats this level)", "[[Sacrifice level up for no reason]]");
                if (statSelection == null) return stat; // User selected cancel
                switch (statSelection)
                {
                    case "MHP":
                        if (stat.MaxHitPoints >= MAX_HIT_POINTS)
                        {
                            Console.WriteLine($"Max hit points cannot exceed {MAX_HIT_POINTS}.");
                            break;
                        }
                        stat.MaxHitPoints += Random.Next(1, 4); // 1-3 hit points per level up
                        stat.MaxHitPoints = Math.Clamp(stat.MaxHitPoints, 1, MAX_HIT_POINTS);
                        stat.HitPoints = stat.MaxHitPoints;
                        statsRemaining--;
                        break;
                    case "CON":
                        if (stat.Constitution >= MAX_STAT_POINTS)
                        {
                            Console.WriteLine($"Constitution cannot exceed {MAX_STAT_POINTS}.");
                            break;
                        }
                        stat.Constitution++;
                        statsRemaining--;
                        break;
                    case "MOV":
                        if (stat.Movement >= MAX_STAT_POINTS)
                        {
                            Console.WriteLine($"Movement cannot exceed {MAX_STAT_POINTS}.");
                            break;
                        }
                        stat.Movement++;
                        statsRemaining--;
                        break;
                    case "STR":
                        if (stat.Strength >= MAX_STAT_POINTS)
                        {
                            Console.WriteLine($"Strength cannot exceed {MAX_STAT_POINTS}.");
                            break;
                        }
                        stat.Strength++;
                        statsRemaining--;
                        break;
                    case "MAG":
                        if (stat.Magic >= MAX_STAT_POINTS)
                        {
                            Console.WriteLine($"Magic cannot exceed {MAX_STAT_POINTS}.");
                            break;
                        }
                        stat.Magic++;
                        statsRemaining--;
                        break;
                    case "DEX":
                        if (stat.Dexterity >= MAX_STAT_POINTS)
                        {
                            Console.WriteLine($"Dexterity cannot exceed {MAX_STAT_POINTS}.");
                            break;
                        }
                        stat.Dexterity++;
                        statsRemaining--;
                        break;
                    case "SPD":
                        if (stat.Speed >= MAX_STAT_POINTS)
                        {
                            Console.WriteLine($"Speed cannot exceed {MAX_STAT_POINTS}.");
                            break;
                        }
                        stat.Speed++;
                        statsRemaining--;
                        break;
                    case "LCK":

                        if (stat.Luck >= MAX_STAT_POINTS)
                        {
                            Console.WriteLine($"Luck cannot exceed {MAX_STAT_POINTS}.");
                            break;
                        }
                        stat.Luck++;
                        statsRemaining--;
                        break;
                    case "DEF":
                        if (stat.Defense >= MAX_STAT_POINTS)
                        {
                            Console.WriteLine($"Defense cannot exceed {MAX_STAT_POINTS}.");
                            break;
                        }
                        stat.Defense++;
                        statsRemaining--;
                        break;
                    case "RES":
                        if (stat.Resistance >= MAX_STAT_POINTS)
                        {
                            Console.WriteLine($"Resistance cannot exceed {MAX_STAT_POINTS}.");
                            break;
                        }
                        stat.Resistance++;
                        statsRemaining--;
                        break;
                    default:
                        throw new InvalidEnumArgumentException($"Invalid selection: {statSelection}");
                }
            }

            levelsRemaining--;
        }

        return stat;
    }

    private Stat GetStartingStatsForClass(string unitClass)
    {
        return unitClass switch
        {
            "Cleric" => new Stat
            {
                MaxHitPoints = 8,
                HitPoints = 8,
                Movement = 5,
                Constitution = 5,
                Strength = 3,
                Magic = 7,
                Dexterity = 4,
                Speed = 5,
                Luck = 4,
                Defense = 1,
                Resistance = 4,
            },
            "Fighter" => new Stat
            {
                MaxHitPoints = 10,
                HitPoints = 10,
                Movement = 5,
                Constitution = 9,
                Strength = 7,
                Magic = 1,
                Dexterity = 5,
                Speed = 7,
                Luck = 4,
                Defense = 3,
                Resistance = 2,
            },
            "Knight" => new Stat
            {
                MaxHitPoints = 12,
                HitPoints = 12,
                Movement = 4,
                Constitution = 10,
                Strength = 6,
                Magic = 1,
                Dexterity = 4,
                Speed = 3,
                Luck = 4,
                Defense = 5,
                Resistance = 1,
            },
            "Rogue" => new Stat
            {
                MaxHitPoints = 10,
                HitPoints = 10,
                Movement = 6,
                Constitution = 6,
                Strength = 4,
                Magic = 2,
                Dexterity = 7,
                Speed = 9,
                Luck = 4,
                Defense = 2,
                Resistance = 3,
            },
            "Wizard" => new Stat
            {
                MaxHitPoints = 8,
                HitPoints = 8,
                Movement = 5,
                Constitution = 5,
                Strength = 1,
                Magic = 7,
                Dexterity = 4,
                Speed = 6,
                Luck = 4,
                Defense = 1,
                Resistance = 3,
            },
            _ => throw new InvalidEnumArgumentException($"Invalid class: {unitClass}"),
        };
    }
}
