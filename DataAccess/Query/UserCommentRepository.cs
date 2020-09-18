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
    class UserCommentRepository : Repository<Domain.UserComment>, IUserCommentRepository
    {
        public UserCommentRepository(DataContext context) : base(context)
        {
        }

        public async Task<int> AddMovieCommentAsync(string userId, int movieId, string comment)
        {
            Domain.UserComment insert = new Domain.UserComment()
            {
                UserId = userId,
                MovieId = movieId,
                Comment = comment
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

        public async Task<string> GetMovieCommentByUserAsync(string userId, int movieId)
        {
            try
            {
                return await Context.UserComments.Where(x => x.UserId == userId && x.MovieId == movieId).Select(x => x.Comment).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RemoveMovieCommentByUserAsync(string userId, int movieId)
        {
            Domain.UserComment commentExists = await Context.UserComments.Where(x => x.UserId == userId && x.MovieId == movieId).FirstOrDefaultAsync();

            if (commentExists == null)
            {
                return false;
            }

            try
            {
                Context.UserComments.Remove(commentExists);
                await Context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
