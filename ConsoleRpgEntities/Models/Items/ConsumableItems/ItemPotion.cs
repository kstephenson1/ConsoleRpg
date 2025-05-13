using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.Items.ConsumableItems;

public class ItemPotion : ConsumableItem, IConsumableItem
{
    // ItemPotion is a consumable item that can be used to restore hit points to a unit. It has a maximum number of
    // uses and can be used by a unit to restore hit points.
    public override string ItemType { get; set; } = "ItemPotion";

    public ItemPotion()
    {
        Name = "Potion";
        Description = "Use to restore 10 hp.";
        MaxDurability = 3;
    }

    public ItemPotion(string name, string desc) : base(name, desc)
    {
        MaxDurability = 3;
    }

    public void UseItem(IUnit unit)
    {
        if (unit!.Stat.HitPoints >= unit.Stat.MaxHitPoints)
        {
            Console.WriteLine($"{unit.Name} is already at max health.");
        }
        else
        {
            int preItemHP = unit.Stat.HitPoints;
            unit.Heal(10);
            int postItemHP = unit.Stat.HitPoints;
            int healedHP = postItemHP - preItemHP;
            Console.WriteLine($"{unit.Name} used {Name} and gained {healedHP} hit points");

            UnitItem unitItem = unit.UnitItems.Where(ui => ui.Item == this).FirstOrDefault()!;
            unitItem.Durability--;

            if (unitItem.Durability == 0)
            {
                Console.WriteLine($"{unit.Name} used the last {Name} and it is now gone.");
                unit.UnitItems.Remove(unitItem);
            }
        }
    }
}
