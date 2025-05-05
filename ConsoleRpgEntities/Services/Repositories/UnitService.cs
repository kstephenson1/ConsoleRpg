using ConsoleRpgEntities.Models.Units.Abstracts;

namespace ConsoleRpgEntities.Services.Repositories;
public class UnitService : IService<Unit>
{
    private readonly IRepository<Unit> _repo;
    public UnitService(IRepository<Unit> repo)
    {
        _repo = repo;
    }
    public void Add(Unit unit)
    {
        _repo.Add(unit);
    }

    public void Delete(Unit unit)
    {
        _repo.Delete(unit);
    }

    public IEnumerable<Unit> GetAll()
    {
        return _repo.GetAll();
    }

    public Unit? GetById(int id)
    {
        return _repo.GetById(id);
    }

    public void Update(Unit unit)
    {
        _repo.Update(unit);
    }

    public void Commit()
    {
        _repo.Commit();
    }
}
