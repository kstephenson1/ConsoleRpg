using ConsoleRpgEntities.Models.Units.Abstracts;

namespace ConsoleRpgEntities.Models.Units.Monsters;

public class EnemyGhost : Monster
{
    public override string UnitType { get; set; } = "EnemyGhost";
    public EnemyGhost()
    {

    }
}
