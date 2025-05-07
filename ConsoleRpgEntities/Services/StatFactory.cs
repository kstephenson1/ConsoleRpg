using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Services.DataHelpers;
using ConsoleRpgEntities.Services.Repositories;

namespace ConsoleRpgEntities.Services;

public class StatFactory
{
    private readonly StatService _statService;
    public StatFactory(StatService statService)
    {
        _statService = statService;
    }
    public Stat CreateStat(IUnit unit)
    {
        int hitPoints = Input.GetInt($"Enter hit points value for {unit.Name}");

        int constitution = Input.GetInt($"Enter constitution for {unit.Name}");
        int movement = 4;

        int strength = Input.GetInt($"Enter strength for {unit.Name}");
        int magic = Input.GetInt($"Enter magic for {unit.Name}");

        int dexterity = Input.GetInt($"Enter dexterity for {unit.Name}");
        int speed = Input.GetInt($"Enter speed for {unit.Name}");
        int luck = Input.GetInt($"Enter luck for {unit.Name}");

        int defense = Input.GetInt($"Enter defense for {unit.Name}");
        int resistance = Input.GetInt($"Enter resistance for {unit.Name}");

        return new Stat
        {
            MaxHitPoints = hitPoints,
            HitPoints = hitPoints,
            Movement = movement,
            Constitution = constitution,
            Strength = strength,
            Magic = magic,
            Dexterity = dexterity,
            Speed = speed,
            Luck = luck,
            Defense = defense,
            Resistance = resistance,
        };
    }
}
