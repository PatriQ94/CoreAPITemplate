using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.Repository
{
    public interface ICarRepository : IRepository<Domain.Car>
    {
        Task<bool> RemoveAsync(int ID);
        Task<long> InsertAsync(string userId, string brand, string model, int year, int kilometers);
        Task<List<Domain.Car>> GetByUserAsync(string userId);
        Task<Domain.Car> CheckIfUserHasCarAsync(string userId, int carId);
        Task<bool> UpdateCarKilometersAsync(int carId, int kilometers);
    }
}
