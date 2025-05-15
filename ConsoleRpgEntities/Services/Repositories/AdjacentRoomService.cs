using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpgEntities.Services.Repositories;
public class AdjacentRoomService : IService<AdjacentRoom>
{
    private readonly IRepository<AdjacentRoom> _repo;
    public AdjacentRoomService(IRepository<AdjacentRoom> repo)
    {
        _repo = repo;
    }
    public void Add(AdjacentRoom room)
    {
        _repo.Add(room);
    }

    public void Delete(AdjacentRoom room)
    {
        _repo.Delete(room);
    }

    public IEnumerable<AdjacentRoom> GetAll()
    {
        return _repo.GetAll();
    }

    public AdjacentRoom? GetById(int id)
    {
        return _repo.GetById(id);
    }

    public void Update(AdjacentRoom room)
    {
        _repo.Update(room);
    }

    public void Commit()
    {
        _repo.Commit();
    }
}
