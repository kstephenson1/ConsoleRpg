using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Helpers;
using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Dungeons;
using ConsoleRpgEntities.Models.Items;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.UI;
using ConsoleRpgEntities.Models.UI.Character;
using ConsoleRpgEntities.Models.UI.Menus;
using ConsoleRpgEntities.Models.UI.Menus.InteractiveMenus;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Services;
using ConsoleRpgEntities.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleRpg;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        // Get the configuration from appsettings.json
        IConfiguration configuration = ConfigurationHelper.GetConfiguration();

        // Register services for dependency injection
        services.AddTransient<AbilityService>();
        services.AddTransient<CharacterUtilities>();
        services.AddTransient<CharacterUI>();
        services.AddTransient<CombatHandler>();
        services.AddTransient<CommandHandler>();
        services.AddTransient<CommandMenu>();
        services.AddTransient<DungeonFactory>();
        services.AddTransient<DungeonService>();
        services.AddTransient<EnemyUnitSelectionMenu>();
        services.AddTransient<ExitMenu>();
        services.AddDbContext<GameContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("DbConnection"))
            .UseLazyLoadingProxies()
            .UseLoggerFactory(MyLoggerFactory)
            .EnableSensitiveDataLogging());
        services.AddTransient<InventoryMenu>();
        services.AddTransient<IRepository<Ability>,Repository<Ability>>();
        services.AddTransient<IRepository<Dungeon>, Repository<Dungeon>>();
        services.AddTransient<IRepository<Item>, Repository<Item>>();
        services.AddTransient<IRepository<Room>, Repository<Room>>();
        services.AddTransient<IRepository<Stat>, Repository<Stat>>();
        services.AddTransient<IRepository<Unit>, Repository<Unit>>();
        services.AddTransient<IRepository<UnitItem>, Repository<UnitItem>>();
        services.AddTransient<ItemCommandMenu>();
        services.AddTransient<ItemService>();
        services.AddTransient<LevelUpCharacterMenu>();
        services.AddTransient<MainMenu>();
        services.AddTransient<MainMenuInventory>();
        services.AddTransient<PartyUnitSelectionMenu>();
        services.AddTransient<RoomFactory>();
        services.AddTransient<RoomMenu>();
        services.AddTransient<RoomNavigationMenu>();
        services.AddTransient<RoomService>();
        services.AddTransient<RoomUI>();
        services.AddTransient<SeedHandler>();
        services.AddTransient<StatFactory>();
        services.AddTransient<StatSelectionMenu>();
        services.AddTransient<StatService>();
        services.AddSingleton<UnitClassSelectionMenu>();
        services.AddTransient<UnitItemService>();
        services.AddTransient<UnitService>();
        services.AddTransient<UserInterface>();
    }

    public static ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
    {
        builder
            .ClearProviders()
            .AddFile("Logs/game-log.txt");
    });
}