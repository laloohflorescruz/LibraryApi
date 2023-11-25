using System.Linq.Expressions;

namespace LibraryApi.Repo;
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    //Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void Update(T entity);
    //void Remove(T entity);
    Task<int> SaveAsync();
}