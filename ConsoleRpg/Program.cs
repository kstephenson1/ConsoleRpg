using System.Reflection;
using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.UI;
using ConsoleRpgEntities.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleRpg;

static class Program
{
    static void Main()
    {
        // Sets the console title to the name of the program, version and author (hey, that's me).
        string? title = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyTitleAttribute>()!.Title;
        string? version = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion.Split('+')[0];
        string? author = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyCompanyAttribute>()!.Company;
        Console.Title = $"{title} v{version} by {author}";

        // Configures the services for dependency injection.
        IServiceCollection serviceCollection = new ServiceCollection();
        Startup.ConfigureServices(serviceCollection);

        // Builds the service provider and retrieves the required services.
        IServiceProvider provider = serviceCollection.BuildServiceProvider();

        CombatHandler combatHandler = provider.GetRequiredService<CombatHandler>();
        SeedHandler seedHandler = provider.GetRequiredService<SeedHandler>();
        UserInterfaceMainMenu mainMenu = provider.GetRequiredService<UserInterfaceMainMenu>();

        // Creates the game engine and starts it. The game engine is the main class that runs the game.
        GameEngine engine = new(combatHandler, mainMenu, seedHandler);
        engine.StartGameEngine();
    }
}