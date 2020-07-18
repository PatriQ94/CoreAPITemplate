using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.Services
{
    public interface ICarService
    {
        Task<string> RemoveAsync(string userId, int carId);
        Task<long> InsertAsync(string userId, string brand, string model, int year, int kilometers);
        Task<List<Domain.Car>> GetByUserAsync(string userId);
        Task<string> Update_KilometersAsync(string userId, int carId, int kilometers);
        //ValueTask<TEntity> GetByIdAsync(int id);
        //Task<IEnumerable<TEntity>> GetAllAsync();
        //IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        //Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        //Task AddAsync(TEntity entity);
        //Task AddRangeAsync(IEnumerable<TEntity> entities);
        //void Remove(TEntity entity);
        //void RemoveRange(IEnumerable<TEntity> entities);
    }
}
