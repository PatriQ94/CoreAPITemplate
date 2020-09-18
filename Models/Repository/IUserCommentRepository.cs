using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.Repository
{
    public interface IUserCommentRepository : IRepository<Domain.UserComment>
    {
        Task<int> AddMovieCommentAsync(string userId, int movieId, string comment);
        Task<string> GetMovieCommentByUserAsync(string userId, int movieId);
        Task<bool> RemoveMovieCommentByUserAsync(string userId, int movieId);
    }
}
