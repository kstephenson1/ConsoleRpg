using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.UnitBehaviors;

namespace ConsoleRpgEntities.Models.Interfaces.UnitClasses;

public interface IArcher : IShootable
{
    // An Archer unit that is able to shoot.
    public void Attack(IUnit target) => Shoot(target);
}
