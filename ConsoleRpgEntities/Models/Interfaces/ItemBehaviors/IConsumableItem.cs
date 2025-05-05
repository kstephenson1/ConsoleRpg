namespace ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

public interface IConsumableItem : IItem
{
    // Interface that allows items to be consumable.
    public int MaxUses { get; set; }
    public int UsesLeft { get; set; }
    public void UseItem(IUnit unit);
}
