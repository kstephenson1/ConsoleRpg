using ConsoleRpgEntities.Models.Items;
using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpgEntities.Services.Repositories;
public class UnitItemService : IService<UnitItem>
{
    private readonly IRepository<UnitItem> _repo;
    public UnitItemService(IRepository<UnitItem> repo)
    {
        _repo = repo;
    }
    public void Add(UnitItem unitItem)
    {
        _repo.Add(unitItem);
    }

    public void Delete(UnitItem unitItem)
    {
        _repo.Delete(unitItem);
    }

    public IEnumerable<UnitItem> GetAll()
    {
        return _repo.GetAll();
    }

    public UnitItem? GetById(int id)
    {
        return _repo.GetById(id);
    }

    public void Update(UnitItem unitItem)
    {
        _repo.Update(unitItem);
    }

    public void Commit()
    {
        _repo.Commit();
    }
}
