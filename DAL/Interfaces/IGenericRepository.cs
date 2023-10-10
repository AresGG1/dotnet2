namespace DAL.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task DeleteAsync(int id);
    Task<T> GetAsync(int id);
    Task<int> AddRangeAsync(IEnumerable<T> list);
    Task ReplaceAsync(T t);
    Task<int> AddAsync(T t);
}
