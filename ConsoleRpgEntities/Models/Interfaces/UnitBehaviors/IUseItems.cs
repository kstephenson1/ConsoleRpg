using ConsoleRpgEntities.Models.Commands.Invokers;
using ConsoleRpgEntities.Models.Commands.ItemCommands;

namespace ConsoleRpgEntities.Models.Interfaces.UnitBehaviors;

public interface IUseItems
{
    // Interface that allows units to hold items.
    CommandInvoker Invoker { get; set; }
    UseItemCommand UseItemCommand { get; set; }
    void UseItem(IItem item);
}
