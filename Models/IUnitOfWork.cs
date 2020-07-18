using Models.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IUnitOfWork : IDisposable
    {
        ICarRepository Car { get; }
        IAuthRepository Auth { get; }

        Task<int> CommitAsync();
    }
}
