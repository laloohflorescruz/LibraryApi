namespace LibraryApi.Repo;
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    void Add(T entity);
    void Update(T entity);
    void Remove(int id);
    Task<int> SaveAsync();
}