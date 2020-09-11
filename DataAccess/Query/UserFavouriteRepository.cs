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
    class UserFavouriteRepository : Repository<Domain.UserFavourite>, IUserFavouriteRepository
    {
        public UserFavouriteRepository(DataContext context) : base(context)
        {
        }

        public async Task<int> AddMovieToFavouritesAsync(string userId, int movieId)
        {
            Domain.UserFavourite insert = new Domain.UserFavourite()
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

        public async Task<bool> RemoveMovieFromFavouritesAsync(string userId, int movieId)
        {
            Domain.UserFavourite favouriteExists = await Context.UserFavourites.Where(x => x.UserId == userId && x.MovieId == movieId).FirstOrDefaultAsync();

            if (favouriteExists == null)
            {
                return false;
            }

            try
            {
                Context.UserFavourites.Remove(favouriteExists);
                await Context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<int>> GetFavouriteMoviesByUserAsync(string userId)
        {
            try
            {
                return await Context.UserFavourites.Where(x => x.UserId == userId).Select(x => x.MovieId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
