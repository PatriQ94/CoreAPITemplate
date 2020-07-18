using Models;
using Models.Domain;
using Models.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Insert a new car for user
        /// </summary>
        public async Task<long> InsertAsync(string userId, string brand, string model, int year, int kilometers)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || year <= 0 || kilometers < 0)
            {
                throw new Exception("Incorrect input parameters, please try again, try again...");
            }

            return await _unitOfWork.Car.InsertAsync(userId, brand, model, year, kilometers);
        }

        /// <summary>
        /// Returns all cars for specific user
        /// </summary>
        public async Task<List<Models.Domain.Car>> GetByUserAsync(string userId)
        {
            List<Models.Domain.Car> response = await _unitOfWork.Car.GetByUserAsync(userId);
            return response;
        }

        /// <summary>
        /// Removes users car by carId
        /// </summary>
        public async Task<string> RemoveAsync(string userId, int carId)
        {
            //Check if user has a car by ID
            var userHasCar = await _unitOfWork.Car.CheckIfUserHasCarAsync(userId, carId);
            if (userHasCar == null)
            {
                throw new Exception($"User does not have a car with the provided ID");
            }

            var removed = await _unitOfWork.Car.RemoveAsync(carId);
            if (removed)
            {
                return "Successfully removed!";
            }
            throw new Exception("Couldn't remove from database");
        }


        /// <summary>
        /// Removes users car kilometers
        /// </summary>
        public async Task<string> Update_KilometersAsync(string userId, int carId, int kilometers)
        {
            //Check if user has a car by ID
            var userHasCar = await _unitOfWork.Car.CheckIfUserHasCarAsync(userId, carId);
            if (userHasCar == null)
            {
                throw new Exception($"User does not have a car with the provided ID");
            }

            var updated = await _unitOfWork.Car.UpdateCarKilometersAsync(carId, kilometers);
            if (updated)
            {
                return "Successfully updated!";
            }
            throw new Exception("Couldn't update the record");
        }
    }
}
