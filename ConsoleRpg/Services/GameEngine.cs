using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.UI;
using ConsoleRpgEntities.Services;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly SeedHandler _seedHandler;
    private readonly UserInterface _userInterface;
    private readonly CombatHandler _combatHandler;

    public GameEngine(SeedHandler seedHandler, UserInterface userInterface, CombatHandler combatHandler)
    {
        _seedHandler = seedHandler;
        _userInterface = userInterface;
        _combatHandler = combatHandler;
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
        _userInterface.MainMenu.Display("[[Start Game]]");

        // Starts the combat handler, which is the main game loop.
        _combatHandler.StartCombat();
    }

    public void End()
    {
        // Ends the game and exits the program.
        _userInterface.ExitMenu.Show();
    }
}
