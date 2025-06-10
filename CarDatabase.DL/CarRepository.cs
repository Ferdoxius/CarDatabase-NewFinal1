using System.Collections.Generic;
using System.Threading.Tasks;
using CarDatabase.Models;
using MongoDB.Driver;

namespace CarDatabase.DL
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllAsync();
        Task<Car?> GetByIdAsync(string id);
        Task CreateAsync(Car car);
        Task<bool> UpdateAsync(Car car);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand);
        Task<IEnumerable<Car>> GetCarsByMaxPriceAsync(decimal maxPrice);
    }

    public class CarRepository : ICarRepository
    {
        private readonly IMongoCollection<Car> _cars;

        public CarRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("CarDatabase");
            _cars = database.GetCollection<Car>("Cars");
        }

        public async Task<IEnumerable<Car>> GetAllAsync() =>
            await _cars.Find(_ => true).ToListAsync();

        public async Task<Car?> GetByIdAsync(string id) =>
            await _cars.Find(car => car.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Car car) =>
            await _cars.InsertOneAsync(car);

        public async Task<bool> UpdateAsync(Car car) =>
            (await _cars.ReplaceOneAsync(c => c.Id == car.Id, car)).IsAcknowledged;

        public async Task<bool> DeleteAsync(string id) =>
            (await _cars.DeleteOneAsync(car => car.Id == id)).DeletedCount > 0;

        public async Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand)
        {
            var filter = Builders<Car>.Filter.Eq(c => c.Brand, brand);
            return await _cars.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByMaxPriceAsync(decimal maxPrice)
        {
            var filter = Builders<Car>.Filter.Lte(c => c.Price, maxPrice);
            return await _cars.Find(filter).ToListAsync();
        }
    }
}
