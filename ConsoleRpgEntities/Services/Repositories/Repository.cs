using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Dungeons;
using ConsoleRpgEntities.Models.Items;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.Units.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRpgEntities.Services.Repositories;
public class Repository<T> : IRepository<T> where T : class, IDatabaseEntity
{
    private readonly GameContext _db;
    public DbSet<T> Entities { get; private set; }
    public Repository(GameContext context)
    {
        _db = context;
        Entities = GetDbSetForEntity();
    }
    public void Add(T entity)
    {
        Entities.Add(entity);
    }

    public void Commit()
    {
        _db.SaveChanges();
    }

    public void Delete(T entity)
    {
        Entities.Remove(entity);
    }

    public IEnumerable<T> GetAll()
    {
        return Entities.AsEnumerable();
    }

    public T? GetById(int id)
    {
        return Entities.Where(e => e.Id == id).FirstOrDefault();
    }

    public void Update(T entity)
    {
        Entities.Update(entity);
    }

#pragma warning disable CS8603 // Possible null reference return.
    private DbSet<T> GetDbSetForEntity()
    {
        return typeof(T).Name switch
        {
            nameof(Dungeon) => _db.Dungeons as DbSet<T>,
            nameof(AdjacentRoom) => _db.AdjacentRooms as DbSet<T>,
            nameof(Room) => _db.Rooms as DbSet<T>,
            nameof(Unit) => _db.Units as DbSet<T>,
            nameof(Stat) => _db.Stats as DbSet<T>,
            nameof(Item) => _db.Items as DbSet<T>,
            nameof(Ability) => _db.Abilities as DbSet<T>,
            nameof(UnitItem) => _db.UnitItems as DbSet<T>,
            _ => throw new NotImplementedException($"DbSet for {nameof(T)} not found")
        };
    }
#pragma warning restore CS8603 // Possible null reference return.
}
