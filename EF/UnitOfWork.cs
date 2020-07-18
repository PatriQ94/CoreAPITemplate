using AutoMapper;
using EF.Query;
using Models;
using Models.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private CarRepository _carRepository;
        private AuthRepository _authRepository;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            this._context = context;
            _mapper = mapper;
        }

        public ICarRepository Car => _carRepository = _carRepository ?? new CarRepository(_context, _mapper);
        public IAuthRepository Auth => _authRepository = _authRepository ?? new AuthRepository(_context, _mapper);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
