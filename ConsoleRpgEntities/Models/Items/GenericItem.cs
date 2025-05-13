namespace ConsoleRpgEntities.Models.Items
{
    public class GenericItem : Item
    {
        // GenericItem is a class that holds item information. It is used to create items that do not have a specific type.
        public override string ItemType { get; set; } = "GenericItem";
        public GenericItem()
        {
            
        }
        public GenericItem(string name, string desc)
            : base(name, desc) { }
    }
}
