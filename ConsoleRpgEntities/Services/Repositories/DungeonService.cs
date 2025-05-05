using ConsoleRpgEntities.Models.Dungeons;

namespace ConsoleRpgEntities.Services.Repositories;
public class DungeonService : IService<Dungeon>
{
    private readonly IRepository<Dungeon> _repo;
    public DungeonService(IRepository<Dungeon> repo)
    {
        _repo = repo;
    }
    public void Add(Dungeon dungeon)
    {
        _repo.Add(dungeon);
    }

    public void Delete(Dungeon dungeon)
    {
        _repo.Delete(dungeon);
    }

    public IEnumerable<Dungeon> GetAll()
    {
        return _repo.GetAll();
    }

    public Dungeon? GetById(int id)
    {
        return _repo.GetById(id);
    }

    public void Update(Dungeon dungeon)
    {
        _repo.Update(dungeon);
    }

    public void Commit()
    {
        _repo.Commit();
    }
}
