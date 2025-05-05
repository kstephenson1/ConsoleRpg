using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Commands.Invokers;
using ConsoleRpgEntities.Models.Commands.UnitCommands;

namespace ConsoleRpgEntities.Models.Interfaces.UnitBehaviors;

public interface ICastable
{
    public Stat Stat { get; set; }
    // Interface that allows units to cast spells.
    CommandInvoker Invoker { set; get; }
    CastCommand CastCommand { set; get; }
    void Cast(string spellName);
}
