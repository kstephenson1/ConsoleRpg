using ConsoleRpgEntities.Models.Combat;

namespace ConsoleRpgEntities.Services.Repositories;
public class StatService : IService<Stat>
{
    private readonly IRepository<Stat> _repo;
    public StatService(IRepository<Stat> repo)
    {
        _repo = repo;
    }
    public void Add(Stat stat)
    {
        _repo.Add(stat);
    }

    public void Delete(Stat stat)
    {
        _repo.Delete(stat);
    }

    public IEnumerable<Stat> GetAll()
    {
        return _repo.GetAll();
    }

    public Stat? GetById(int id)
    {
        return _repo.GetById(id);
    }

    public void Update(Stat stat)
    {
        _repo.Update(stat);
    }

    public void Commit()
    {
        _repo.Commit();
    }
}
