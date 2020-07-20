using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Query
{
    class AuthRepository : Repository<Domain.RefreshToken>, IAuthRepository
    {
        private readonly IMapper _mapper;

        public AuthRepository(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            RefreshToken response = null;

            try
            {
                Domain.RefreshToken token = await Context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

                //Map result
                if (token != null)
                {
                    response = _mapper.Map<RefreshToken>(token);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<string> SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            Domain.RefreshToken insert = _mapper.Map<Domain.RefreshToken>(refreshToken);

            try
            {
                //Insert to database and save changes
                await Context.AddAsync(insert);
                await Context.SaveChangesAsync();

                return insert.Token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateRefreshTokenAsync(RefreshToken storedRefreshToken)
        {
            Domain.RefreshToken toUpdate = await Context.RefreshTokens.FirstOrDefaultAsync(c => c.Token == storedRefreshToken.Token);
            if (toUpdate == null)
            {
                return false;
            }

            try
            {
                //Update record and save changes
                toUpdate.Used = storedRefreshToken.Used;
                Context.RefreshTokens.Update(toUpdate);
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
