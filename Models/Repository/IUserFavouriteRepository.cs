using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.Repository
{
    public interface IUserFavouriteRepository : IRepository<Domain.UserFavourite>
    {
        Task<int> AddMovieToFavouritesAsync(string userId, int movieId);
        Task<bool> RemoveMovieFromFavouritesAsync(string userId, int movieId);
        Task<List<int>> GetFavouriteMoviesByUserAsync(string userId);
    }
}
