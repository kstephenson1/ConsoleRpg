using ConsoleRpgEntities.Models.Units.Abstracts;

namespace ConsoleRpgEntities.Models.Units.Monsters;

public class EnemyGoblin : Monster
{
    public override string UnitType { get; set; } = "EnemyGoblin";
    public EnemyGoblin()
    {

    }
}
