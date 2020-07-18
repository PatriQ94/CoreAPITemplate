using Models.Contracts.Responses;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Models.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResult> GenerateJWTAccessToken(IdentityUser user);
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string accessToken, string refreshToken);
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
