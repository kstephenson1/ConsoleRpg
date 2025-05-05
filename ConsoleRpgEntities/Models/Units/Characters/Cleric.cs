using System.ComponentModel.DataAnnotations.Schema;
using ConsoleRpgEntities.Models.Commands.UnitCommands;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Models.Interfaces.UnitClasses;

namespace ConsoleRpgEntities.Models.Units.Characters;

public class Cleric : Character, ICleric
{
    public override string UnitType { get; set; } = "Cleric";

    public Cleric()
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
