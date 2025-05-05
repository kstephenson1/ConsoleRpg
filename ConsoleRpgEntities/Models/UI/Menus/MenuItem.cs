namespace ConsoleRpgEntities.Models.UI.Menus;
public class MenuItem
{
    // MenuItem is an object to be stored in a menu.  It holds an index and a name.
    public int Index { get; protected set; }
    public string Name { get; set; }

    public MenuItem(int index, string name)
    {
        Index = index;
        Name = name;
    }
}
