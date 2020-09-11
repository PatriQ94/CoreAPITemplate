using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Query
{
    class UserSeenRepository : Repository<Domain.UserSeen>, IUserSeenRepository
    {
        public UserSeenRepository(DataContext context) : base(context)
        {
        }

        public async Task<int> AddMovieToSeenAsync(string userId, int movieId)
        {
            Domain.UserSeen insert = new Domain.UserSeen()
            {
                UserId = userId,
                MovieId = movieId
            };

            try
            {
                await Context.AddAsync(insert);
                await Context.SaveChangesAsync();

                return insert.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RemoveMovieFromSeenListAsync(string userId, int movieId)
        {
            Domain.UserSeen seenExists = await Context.UserSeens.Where(x => x.UserId == userId && x.MovieId == movieId).FirstOrDefaultAsync();

            if (seenExists == null)
            {
                return false;
            }

            try
            {
                Context.UserSeens.Remove(seenExists);
                await Context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<int>> GetSeenMoviesByUserAsync(string userId)
        {
            try
            {
                return await Context.UserSeens.Where(x => x.UserId == userId).Select(x => x.MovieId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
