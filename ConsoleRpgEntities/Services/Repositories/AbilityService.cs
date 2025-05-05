using ConsoleRpgEntities.Models.Abilities;

namespace ConsoleRpgEntities.Services.Repositories;
public class AbilityService : IService<Ability>
{
    private readonly IRepository<Ability> _repo;
    public AbilityService(IRepository<Ability> repo)
    {
        _repo = repo;
    }
    public void Add(Ability ability)
    { 
        _repo.Add(ability);
    }

    public void Delete(Ability ability)
    {
        _repo.Delete(ability);
    }

    public IEnumerable<Ability> GetAll()
    {
        return _repo.GetAll();
    }

    public Ability? GetById(int id)
    {
        return _repo.GetById(id);
    }

    public void Update(Ability ability)
    {
        _repo.Update(ability);
    }

    public void Commit()
    {
        _repo.Commit();
    }
}
