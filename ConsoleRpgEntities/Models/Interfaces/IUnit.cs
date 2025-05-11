using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Commands.AbilityCommands;
using ConsoleRpgEntities.Models.Commands.UnitCommands;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.Interfaces.InventoryBehaviors;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;
using ConsoleRpgEntities.Models.Interfaces.UnitBehaviors;

namespace ConsoleRpgEntities.Models.Interfaces;

public interface IUnit : ITargetable, IAttack, IHaveInventory, IUseItems
{
    // Interface that allows units to exist.
    public int Id { get; set; }
    MoveCommand MoveCommand { set; get; }
    AbilityCommand AbilityCommand { set; get; }
    public string Name { get; set; }
    public string Class { get; set; }
    public int Level { get; set; }
    Room? CurrentRoom { get; set; }
    public Stat Stat { get; set; }
    public List<Ability> Abilities { get; }

    void Move();
    void UseAbility(Ability ability, IUnit target);
    string GetHealthBar();
    public IEquippableWeapon? GetEquippedWeapon();
    public IEquippableArmor? GetEquippedArmorInSlot(ArmorType armorType);
}
