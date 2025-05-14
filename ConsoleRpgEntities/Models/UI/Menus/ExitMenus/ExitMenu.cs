using ConsoleRpgEntities.Models.UI.Menus.MenuItems;
using Spectre.Console;

namespace ConsoleRpgEntities.Models.UI.Menus.ExitMenus;

public class ExitMenu : Menu
{
    // ExitMenu is used to display a message to the user when they _exit the game.  It is used to thank the user for
    // using the game and to provide any other information that may be relevant.

    //protected Table _table = new();
    protected List<MenuItem> _menuItems = new();

    public ExitMenu()
    {
        Update();
        BuildTable();
    }

    public void Display()
    {
        Update();
        Show();
    }

    public void Update()
    {
        _menuItems = new();
        AddMenuItem("                                                                      ");
        AddMenuItem("           Thank you for using the RPG Character Simulator.           ");
        AddMenuItem("                              By Kyle S.                              ");
        AddMenuItem("                                                                      ");
    }
}

