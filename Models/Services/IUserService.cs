using Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.Services
{
    public interface IUserService
    {
        Task<int> AddMovieToFavouritesAsync(string userId, int movieId);
        Task<string> RemoveMovieFromFavouritesAsync(string userId, int movieId);
        Task<List<Movie>> GetFavouriteMoviesByUserAsync(string userId);
        Task<int> AddMovieToSeenAsync(string userId, int movieId);
        Task<string> RemoveMovieFromSeenListAsync(string userId, int movieId);
        Task<List<Movie>> GetSeenMoviesByUserAsync(string userId);
        Task<int> AddMovieCommentAsync(string userId, int movieId, string comment);
        Task<string> GetMovieCommentByUserAsync(string userId, int movieId);
        Task<string> RemoveMovieCommentByUserAsync(string userId, int movieId);
    }
}
