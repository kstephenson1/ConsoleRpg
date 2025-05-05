using ConsoleRpgEntities.Models.Commands.Invokers;
using ConsoleRpgEntities.Models.Commands.ItemCommands;
using ConsoleRpgEntities.Models.Commands.UnitCommands;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.Interfaces.UnitBehaviors;

public interface IAttack
{
    // Interface that allows units to attack.
    CommandInvoker Invoker { set; get; }
    AttackCommand AttackCommand { set; get; }
    EquipCommand EquipCommand { set; get; }

    void Attack(IUnit target);
    void Equip(IEquippableItem item);
    void Unequip(IEquippableItem item);
}
