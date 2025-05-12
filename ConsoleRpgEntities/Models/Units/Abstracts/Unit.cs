using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Commands.AbilityCommands;
using ConsoleRpgEntities.Models.Commands.Invokers;
using ConsoleRpgEntities.Models.Commands.ItemCommands;
using ConsoleRpgEntities.Models.Commands.UnitCommands;
using ConsoleRpgEntities.Models.Items;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.InventoryBehaviors;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;
using ConsoleRpgEntities.Models.Interfaces.UnitBehaviors;
using ConsoleRpgEntities.Services.Repositories;

namespace ConsoleRpgEntities.Models.Units.Abstracts;

public abstract class Unit : IUnit, ITargetable, IAttack, IHaveInventory, IDatabaseEntity
{
    // Unit is an abstract class that holds basic unit properties and functions.
    [Key]
    public int Id { get; set; }
    public abstract string UnitType { get; set; }
    public virtual string Name { get; set; }
    public virtual string Class { get; set; }
    public virtual int Level { get; set; }
    //public virtual List<Item> Items { get; set; }
    public virtual List<UnitItem> UnitItems { get; set; }
    public Room? CurrentRoom { get; set; }
    [NotMapped]
    public virtual CommandInvoker Invoker { get; set; } = new();
    [NotMapped]
    public virtual UseItemCommand UseItemCommand { get; set; } = null!;
    [NotMapped]
    public virtual UnequipCommand UnequipCommand { get; set; } = null!;
    [NotMapped]
    public virtual EquipCommand EquipCommand { get; set; } = null!;
    [NotMapped]
    public virtual DropItemCommand DropItemCommand { get; set; } = null!;
    [NotMapped]
    public virtual TradeItemCommand TradeItemCommand { get; set; } = null!;
    [NotMapped]
    public virtual AttackCommand AttackCommand { get; set; } = null!;
    [NotMapped]
    public virtual MoveCommand MoveCommand { get; set; } = null!;
    [NotMapped]
    public virtual AbilityCommand AbilityCommand { get; set; } = null!;

    public virtual Stat Stat { get; set; }
    public virtual List<Ability> Abilities { get; } = new();

    public Unit()
    {
        
    }

    public Unit(string name, string characterClass, int level, List<Item> items, Stat stats)
    {
        Name = name;
        Class = characterClass;
        Level = level;
        //Items = items;
        Stat = stats;
    }

    // Attacks the target unit.
    public virtual void Attack(IUnit target)
    {
        AttackCommand = new(this, target);
        Invoker.ExecuteCommand(AttackCommand);
    }

    // Moves the unit to a position.
    public virtual void Move()
    {
        MoveCommand = new(this);
        Invoker.ExecuteCommand(MoveCommand);
    }

    // Has the unit take damage then check if it is dead.
    public virtual void Damage(int damage)
    {
        Stat.HitPoints -= damage;
        OnHealthChanged();

        if (IsDead())
            OnDeath();
    }

    public virtual void Heal(int heal)
    {
        Stat.HitPoints += heal;
        OnHealthChanged();
    }

    // Triggers every time this unit takes damage.
    public virtual void OnHealthChanged()
    {
        if (Stat.HitPoints > Stat.MaxHitPoints)
            Stat.HitPoints = Stat.MaxHitPoints;
        if (Stat.HitPoints <= 0)
            Stat.HitPoints = 0;
    }

    // Triggers when this unit dies.
    public virtual void OnDeath()
    {

    }

    // Function to check to see if unit should be dead.
    public bool IsDead()
    {
        return Stat.HitPoints <= 0;
    }

    public override string ToString()
    {
        return $"{Name},{Class},{Level},{Stat.HitPoints}";
    }

    public string GetHealthBar()
    {
        string bar = "[[";
        for (int i = 0; i < Stat.MaxHitPoints; i++)
        {
            if (i != 0 && i % 30 == 0)
                bar += "\n  ";
            if (i < Stat.HitPoints)
                bar += "[green]■[/]";
            else
                bar += "[red3]■[/]";
        }
        bar += "]]";

        if (Stat.HitPoints <= 0)
        {
            return $"[dim]{bar}[/]";
        }
        return bar;
    }

    public void Equip(IEquippableItem item)
    {
        EquipCommand = new(this, item);
        Invoker.ExecuteCommand(EquipCommand);
    }

    public void Unequip(IEquippableItem item)
    {
        UnequipCommand = new(this, item);
        Invoker.ExecuteCommand(UnequipCommand);
    }

    public void DropItem(IItem item)
    {
        DropItemCommand = new(this, item);
        Invoker.ExecuteCommand(DropItemCommand);
    }

    public void TradeItem(IItem item, IUnit target, UnitItemService unitItemService)
    {
        TradeItemCommand = new(this, item, target, unitItemService);
        Invoker.ExecuteCommand(TradeItemCommand);
    }

    public void UseItem(IItem item)
    {
        UseItemCommand = new(this, item);
        Invoker.ExecuteCommand(UseItemCommand);
    }

    public void UseAbility(Ability ability, IUnit target)
    {
        AbilityCommand = new(ability, this, target);
        Invoker.ExecuteCommand(AbilityCommand);
    }

    public IEquippableWeapon? GetEquippedWeapon() => InventoryHelper.GetEquippedWeapon(this);
    public IEquippableArmor? GetEquippedArmorInSlot(ArmorType armorType) => InventoryHelper.GetEquippedArmorInSlot(this, armorType);
    public int GetMaxCarryWeight() => Stat.GetMaxCarryWeight();
    public int GetCurrentCarryWeight() => InventoryHelper.GetCurrentCarryWeight(this);

}
