using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.Repository
{
    public interface IUserSeenRepository : IRepository<Domain.UserSeen>
    {
        Task<int> AddMovieToSeenAsync(string userId, int movieId);
        Task<bool> RemoveMovieFromSeenListAsync(string userId, int movieId);
        Task<List<int>> GetSeenMoviesByUserAsync(string userId);
    }
}
