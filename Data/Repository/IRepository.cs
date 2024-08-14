using System.Linq.Expressions;

namespace SpreadSheet.Data.Repository
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);

        Task<T> GetByNameAsync(Expression<Func<T, bool>> filter);

        Task<T> CreateAsync(T dbRecord);

        Task<T> UpdateAsync(T student);

        Task<bool> DeleteAsync(T dbRecord);

        Task<int> Count();

        List<T> GetPaginated(int page, int size);

        Task<T> GetLastAsync();
    }
}
