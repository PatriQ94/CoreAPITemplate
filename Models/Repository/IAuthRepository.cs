using Models.Domain;
using System.Threading.Tasks;

namespace Models.Repository
{
    public interface IAuthRepository : IRepository<RefreshToken>
    {
        Task<Domain.RefreshToken> GetRefreshTokenAsync(string refreshToken);
        Task<bool> UpdateRefreshTokenAsync(RefreshToken storedRefreshToken);

        Task<string> SaveRefreshTokenAsync(RefreshToken refreshToken);
    }
}
