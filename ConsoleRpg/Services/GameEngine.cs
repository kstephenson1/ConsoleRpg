using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.UI;
using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;
using ConsoleRpgEntities.Services;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly CombatHandler _combatHandler;
    private readonly SeedHandler _seedHandler;
    private readonly UserInterface _userInterface;
    private readonly UserInterfaceMainMenu _mainMenu;

    public GameEngine(CombatHandler combatHandler, UserInterfaceMainMenu mainMenu, SeedHandler seedHandler, UserInterface userInterface)
    {
        _combatHandler = combatHandler;
        _mainMenu = mainMenu;
        _seedHandler = seedHandler;
        _userInterface = userInterface;
    }

    public void StartGameEngine()
    {
        Initialization();
        Run();
        End();
    }

    public void Initialization()
    {
        // Seeds the database with initial data. This is only run once when the program is started for the first time.
        _seedHandler.SeedDatabase();
    }

    public void Run()
    {
        // Runs the main game loop. This is where the game starts and runs until the user chooses to exit.

        // Shows the main menu and waits for the user to choose an option.
        _mainMenu.MainMenu.Display("[[Start Game]]");

        // Starts the combat handler, which is the main game loop.
        _combatHandler.StartCombat();
    }

    public void End()
    {
        // Ends the game and exits the program.
        _mainMenu.Exit.Show();
    }
}
