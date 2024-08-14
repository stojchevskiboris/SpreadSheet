using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SpreadSheet.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SpreadSheetDbContext _dbContext;
        private readonly DbSet<T> _dbSet;


        public Repository(SpreadSheetDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false)
        {
            if (useNoTracking)
            {
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            }
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetByNameAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).FirstOrDefaultAsync();

        }

        public async Task<T> CreateAsync(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _dbContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<T> UpdateAsync(T dbRecord)
        {
            _dbSet.Update(dbRecord);

            _dbContext.Entry(dbRecord).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return dbRecord;
        }

        public async Task<bool> DeleteAsync(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> Count()
        {
            return await _dbSet.CountAsync();
        }

        public List<T> GetPaginated(int page, int size)
        {
            var records = _dbSet.ToList();
            
            return records.Skip((page - 1) * size).Take(size).ToList();


        }

        public async Task<T> GetLastAsync()
        {
            return await _dbSet.Reverse().FirstOrDefaultAsync();
            
        }
    }
}
