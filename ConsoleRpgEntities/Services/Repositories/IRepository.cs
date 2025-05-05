using Microsoft.EntityFrameworkCore;

namespace ConsoleRpgEntities.Services.Repositories;

public interface IRepository<T> where T : class, IDatabaseEntity
{
    public DbSet<T> Entities { get; }
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    T? GetById(int id);
    IEnumerable<T> GetAll();
    void Commit();
}
