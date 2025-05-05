using System.ComponentModel.DataAnnotations.Schema;
using ConsoleRpgEntities.Models.Commands.UnitCommands;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Models.Interfaces.UnitClasses;

namespace ConsoleRpgEntities.Models.Units.Monsters;

public class EnemyMage : Monster, IMage
{
    public override string UnitType { get; set; } = "EnemyMage";
    public EnemyMage()
    {

    }

    [NotMapped]
    public virtual CastCommand CastCommand { get; set; } = null!;

    public void Cast(string spellName)
    {
        CastCommand = new(this, spellName);
        Invoker.ExecuteCommand(CastCommand);
    }
}
