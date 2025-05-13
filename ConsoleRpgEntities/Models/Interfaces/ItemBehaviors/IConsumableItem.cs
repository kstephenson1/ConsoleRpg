namespace ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

public interface IConsumableItem : IItem
{
    // Interface that allows items to be consumable.
    public void UseItem(IUnit unit);
}
