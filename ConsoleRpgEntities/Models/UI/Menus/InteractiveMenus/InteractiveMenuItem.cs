namespace ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;
public class InteractiveMenuItem : MenuItem
{
    // The MainMenuItem is used to store information about the selection in the main menu.  This stores the index and name (from the base),
    // description, and an acion in a handy object that can be referenced easily later.
    public string Description { get; set; }

    public InteractiveMenuItem(int index, string name, string desc) : base(index, name)
    {
        Index = index;
        Name = name;
        Description = desc;
    }
}
