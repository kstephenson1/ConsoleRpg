namespace ConsoleRpgEntities.Models.UI.Menus.MenuItems;
public class InteractiveSelectionMenuItem<T> : InteractiveMenuItem
{
    // The InteractiveReturnMenuItem is used to store information about the selection in the main menu.  This stores the index and name (from the base),
    // description, and a generic type.
    public T Selection { get; set; }

    public InteractiveSelectionMenuItem(int index, string name, string desc, T selection) : base(index, name, desc)
    {
        Index = index;
        Name = name;
        Description = desc;
        Selection = selection;
    }
}
