using Models;
using Services.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class CarServiceTests
    {
        private readonly CarService _carService;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        public CarServiceTests()
        {
            _carService = new CarService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldReturnCarId()
        {
            //Arrange
            string userId = Guid.NewGuid().ToString();
            string brand = "Volkswagen";
            string model = "Golf 5";
            int year = 2020;
            int kilometers = 5000;
            long expected = 3;
            _unitOfWorkMock.Setup(x => x.Car.InsertAsync(userId, brand, model, year, kilometers)).ReturnsAsync(expected);

            //Act
            var actual = await _carService.InsertAsync(userId, brand, model, year, kilometers);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task InsertAsync_ShouldThrowException_WhenInputParametersAreInCorrect()
        {
            //Arrange
            string userId = Guid.NewGuid().ToString();
            string brand = "Volkswagen";
            string model = "Golf 5";
            int year = 2020;
            int kilometers = 5000;
            long carId = 3;
            string expected = "Incorrect input parameters, please try again, try again...";
            _unitOfWorkMock.Setup(x => x.Car.InsertAsync(userId, brand, model, year, kilometers)).ReturnsAsync(carId);

            //Act
            Task actual() => _carService.InsertAsync("", "", "", year, kilometers);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(actual);
            Assert.Equal(expected, ex.Message);
        }

        [Fact]
        public async Task GetByUserAsync_ShouldReturnOneCar()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            List<Models.Domain.Car> userCars = new List<Models.Domain.Car>()
            {
                new Models.Domain.Car(){
                    UserId = userId,
                    ID = 1,
                    Brand = "Volkswagen",
                    Kilometers = 5000,
                    Year = 2020,
                    Model = "Golf 5"
                }
            };
            _unitOfWorkMock.Setup(x => x.Car.GetByUserAsync(userId)).ReturnsAsync(userCars);

            //Act
            var actual = await _carService.GetByUserAsync(userId);

            //Assert
            Assert.Single(actual);
            Assert.Equal(userId, actual[0].UserId);
            Assert.Equal("Golf 5",actual[0].Model);
        }

        [Fact]
        public async Task GetByUserAsync_ShouldReturnEmpty_WhenUserDoesntExistOrHasNoCars()
        {
            //Arrange
            _unitOfWorkMock.Setup(x => x.Car.GetByUserAsync(It.IsAny<string>())).ReturnsAsync(new List<Models.Domain.Car>());

            //Act
            var actual = await _carService.GetByUserAsync(Guid.NewGuid().ToString());

            //Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task RemoveAsync_ShouldReturnSuccessfulString()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            int carId = 3;
            string expected = "Successfully removed!";
            Models.Domain.Car userHasCar = new Models.Domain.Car()
            {
                ID = carId
            };
            _unitOfWorkMock.Setup(x => x.Car.CheckIfUserHasCarAsync(userId, carId)).ReturnsAsync(userHasCar);
            _unitOfWorkMock.Setup(x => x.Car.RemoveAsync(userHasCar.ID)).ReturnsAsync(true);

            //Act
            var actual = await _carService.RemoveAsync(userId, carId);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task RemoveAsync_ShouldThrowException_WhenUserDoesntHaveCarByID()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            int carId = 3;
            string expected = $"User does not have a car with the provided ID";
            Models.Domain.Car userHasCar = new Models.Domain.Car()
            {
                ID = carId
            };
            _unitOfWorkMock.Setup(x => x.Car.CheckIfUserHasCarAsync(userId, carId)).ReturnsAsync(() => null);

            //Act
            Task actual() => _carService.RemoveAsync(userId, carId);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(actual);
            Assert.Equal(expected, ex.Message);
        }

        [Fact]
        public async Task RemoveAsync_ShouldThrowException_WhenRemovingFromDatabaseFails()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            int carId = 3;
            string expected = $"Couldn't remove from database";
            Models.Domain.Car userHasCar = new Models.Domain.Car()
            {
                ID = carId
            };
            _unitOfWorkMock.Setup(x => x.Car.CheckIfUserHasCarAsync(userId, carId)).ReturnsAsync(userHasCar);
            _unitOfWorkMock.Setup(x => x.Car.RemoveAsync(userHasCar.ID)).ReturnsAsync(false);

            //Act
            Task actual() => _carService.RemoveAsync(userId, carId);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(actual);
            Assert.Equal(expected, ex.Message);
        }

        [Fact]
        public async Task Update_KilometersAsync_ShouldReturnSuccessfulString()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            int carId = 3;
            int kilometers = 5000;
            string expected = "Successfully updated!";
            Models.Domain.Car userHasCar = new Models.Domain.Car()
            {
                ID = carId
            };
            _unitOfWorkMock.Setup(x => x.Car.CheckIfUserHasCarAsync(userId, carId)).ReturnsAsync(userHasCar);
            _unitOfWorkMock.Setup(x => x.Car.UpdateCarKilometersAsync(userHasCar.ID, kilometers)).ReturnsAsync(true);

            //Act
            var actual = await _carService.Update_KilometersAsync(userId, userHasCar.ID, kilometers);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Update_KilometersAsync_ShouldThrowException_WhenUserDoesntHaveCarByID()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            int carId = 3;
            int kilometers = 5000;
            string expected = $"User does not have a car with the provided ID";
            Models.Domain.Car userHasCar = new Models.Domain.Car()
            {
                ID = carId
            };
            _unitOfWorkMock.Setup(x => x.Car.CheckIfUserHasCarAsync(userId, carId)).ReturnsAsync(() => null);

            //Act
            Task actual() => _carService.Update_KilometersAsync(userId, userHasCar.ID, kilometers);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(actual);
            Assert.Equal(expected, ex.Message);
        }

        [Fact]
        public async Task Update_KilometersAsync_ShouldThrowException_WhenUpdatingKilometersFails()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            int carId = 3;
            string expected = $"Couldn't update the record";
            int kilometers = 5000;
            Models.Domain.Car userHasCar = new Models.Domain.Car()
            {
                ID = carId
            };
            _unitOfWorkMock.Setup(x => x.Car.CheckIfUserHasCarAsync(userId, carId)).ReturnsAsync(userHasCar);
            _unitOfWorkMock.Setup(x => x.Car.UpdateCarKilometersAsync(userHasCar.ID, kilometers)).ReturnsAsync(false);

            //Act
            Task actual() => _carService.Update_KilometersAsync(userId, userHasCar.ID, kilometers);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(actual);
            Assert.Equal(expected, ex.Message);
        }
    }
}
