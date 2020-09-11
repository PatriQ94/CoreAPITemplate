using AutoMapper;
using DataAccess.Query;
using Models;
using Models.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private CarRepository _carRepository;
        private AuthRepository _authRepository;
        private UserCommentRepository _userCommentRepository;
        private UserFavouriteRepository _userFavouriteRepository;
        private UserSeenRepository _userSeenRepository;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            this._context = context;
            _mapper = mapper;
        }

        public ICarRepository Car => _carRepository = _carRepository ?? new CarRepository(_context, _mapper);
        public IAuthRepository Auth => _authRepository = _authRepository ?? new AuthRepository(_context, _mapper);

        public IUserCommentRepository UserComments => _userCommentRepository = _userCommentRepository ?? new UserCommentRepository(_context);
        public IUserFavouriteRepository UserFavourites => _userFavouriteRepository = _userFavouriteRepository ?? new UserFavouriteRepository(_context);
        public IUserSeenRepository UserSeens => _userSeenRepository = _userSeenRepository ?? new UserSeenRepository(_context);

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
