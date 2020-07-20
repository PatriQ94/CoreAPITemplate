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
    class CarRepository : Repository<Domain.Car>, ICarRepository
    {
        private readonly IMapper _mapper;

        public CarRepository(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<long> InsertAsync(string userId, string brand, string model, int year, int kilometers)
        {
            Domain.Car insert = new Domain.Car()
            {
                UserId = userId,
                Brand = brand,
                Model = model,
                Year = year,
                Kilometers = kilometers
            };

            try
            {
                //Insert to database and save changes
                await Context.AddAsync(insert);
                await Context.SaveChangesAsync();

                return insert.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Models.Domain.Car>> GetByUserAsync(string userId)
        {
            List<Models.Domain.Car> response = new List<Models.Domain.Car>();

            try
            {
                //Get cars for user
                List<Domain.Car> data = await Context.Car.Where(x => x.UserId == userId).ToListAsync();

                //Map result
                response = _mapper.Map<List<Models.Domain.Car>>(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<bool> RemoveAsync(int ID)
        {
            Domain.Car toDelete = new Domain.Car()
            {
                ID = ID
            };

            try
            {
                Context.Car.Remove(toDelete);
                await Context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Models.Domain.Car> CheckIfUserHasCarAsync(string userId, int carId)
        {
            Domain.Car carExists = await Context.Car.Where(x => x.UserId == userId && x.ID == carId).FirstOrDefaultAsync();

            if (carExists == null)
            {
                return null;
            }

            return _mapper.Map<Models.Domain.Car>(carExists);
        }

        public async Task<bool> UpdateCarKilometersAsync(int carId, int kilometers)
        {
            Domain.Car toUpdate = await Context.Car.FirstOrDefaultAsync(c => c.ID == carId);
            if (toUpdate == null)
            {
                return false;
            }

            try
            {
                toUpdate.Kilometers = kilometers;
                Context.Car.Update(toUpdate);
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
