using System.Collections.Generic;
using System.Threading.Tasks;
using CarDatabase.DL;
using CarDatabase.Models;

namespace CarDatabase.BL
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAllAsync();
        Task<Car?> GetByIdAsync(string id);
        Task CreateAsync(Car car);
        Task<bool> UpdateAsync(Car car);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand);
        Task<IEnumerable<Car>> GetCarsByMaxPriceAsync(decimal maxPrice);
    }

    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<IEnumerable<Car>> GetAllAsync() => await _carRepository.GetAllAsync();
        public async Task<Car?> GetByIdAsync(string id) => await _carRepository.GetByIdAsync(id);
        public async Task CreateAsync(Car car) => await _carRepository.CreateAsync(car);
        public async Task<bool> UpdateAsync(Car car) => await _carRepository.UpdateAsync(car);
        public async Task<bool> DeleteAsync(string id) => await _carRepository.DeleteAsync(id);
        public async Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand) => await _carRepository.GetCarsByBrandAsync(brand);
        public async Task<IEnumerable<Car>> GetCarsByMaxPriceAsync(decimal maxPrice) => await _carRepository.GetCarsByMaxPriceAsync(maxPrice);
    }
}
