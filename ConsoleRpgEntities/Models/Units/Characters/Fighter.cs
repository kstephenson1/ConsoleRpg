using ConsoleRpgEntities.Models.Units.Abstracts;

namespace ConsoleRpgEntities.Models.Units.Characters;

public class Fighter : Character
{
    public override string UnitType { get; set; } = "Fighter";

    public Fighter()
    {

    }
}
