using CarDatabase.BL;
using CarDatabase.DL;
using CarDatabase.Models;
using Moq;
using Xunit;

namespace CarDatabase.test
{
    public class CarServiceTests
    {
        private readonly Mock<ICarRepository> _carRepositoryMock;
        private readonly ICarService _carService;

        public CarServiceTests()
        {
            _carRepositoryMock = new Mock<ICarRepository>();
            _carService = new CarService(_carRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllCars_ReturnsEmptyList_WhenNoCarsExist()
        {
            _carRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Car>());

            var result = await _carService.GetAllAsync();


            Assert.Empty(result);
        }
    }
}
