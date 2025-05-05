using Microsoft.EntityFrameworkCore;

namespace ConsoleRpgEntities.Services.Repositories;

public interface IService<T> where T : class, IDatabaseEntity
{
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    T? GetById(int id);
    IEnumerable<T> GetAll();
}
