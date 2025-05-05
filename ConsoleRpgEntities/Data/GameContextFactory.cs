using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ConsoleRpgEntities.Helpers;

namespace ConsoleRpgEntities.Data
{
    public class GameContextFactory : IDesignTimeDbContextFactory<GameContext>
    {
        public GameContext CreateDbContext(string[] args)
        {
            // Get configuration
            IConfigurationRoot configuration = ConfigurationHelper.GetConfiguration();

            // Get connection string
            string? connectionString = configuration.GetConnectionString("DbConnection");

            // Build DbContextOptions
            DbContextOptionsBuilder<GameContext> optionsBuilder = new DbContextOptionsBuilder<GameContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Create and return the GameContext
            return new GameContext(optionsBuilder.Options);
        }
    }
}