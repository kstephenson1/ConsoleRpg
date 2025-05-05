using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpgEntities.Services.Repositories;
public class RoomService : IService<Room>
{
    private readonly IRepository<Room> _repo;
    public RoomService(IRepository<Room> repo)
    {
        _repo = repo;
    }
    public void Add(Room room)
    {
        _repo.Add(room);
    }

    public void Delete(Room room)
    {
        _repo.Delete(room);
    }

    public IEnumerable<Room> GetAll()
    {
        return _repo.GetAll();
    }

    public Room? GetById(int id)
    {
        return _repo.GetById(id);
    }

    public void Update(Room room)
    {
        _repo.Update(room);
    }

    public void Commit()
    {
        _repo.Commit();
    }
}
