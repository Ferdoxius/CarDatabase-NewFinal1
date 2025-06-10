using System.Collections.Generic;
using System.Threading.Tasks;
using CarDatabase.Models;
using MongoDB.Driver;

namespace CarDatabase.DL
{
    public interface IManufacturerRepository
    {
        Task<IEnumerable<Manufacturer>> GetAllAsync();
        Task<Manufacturer?> GetByIdAsync(string id);
        Task CreateAsync(Manufacturer manufacturer);
        Task<bool> UpdateAsync(Manufacturer manufacturer);
        Task<bool> DeleteAsync(string id);
    }

    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly IMongoCollection<Manufacturer> _manufacturers;

        public ManufacturerRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("CarDatabase");
            _manufacturers = database.GetCollection<Manufacturer>("Manufacturers");
        }

        public async Task<IEnumerable<Manufacturer>> GetAllAsync() =>
            await _manufacturers.Find(_ => true).ToListAsync();

        public async Task<Manufacturer?> GetByIdAsync(string id) =>
            await _manufacturers.Find(m => m.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Manufacturer manufacturer) =>
            await _manufacturers.InsertOneAsync(manufacturer);

        public async Task<bool> UpdateAsync(Manufacturer manufacturer) =>
            (await _manufacturers.ReplaceOneAsync(m => m.Id == manufacturer.Id, manufacturer)).IsAcknowledged;

        public async Task<bool> DeleteAsync(string id) =>
            (await _manufacturers.DeleteOneAsync(m => m.Id == id)).DeletedCount > 0;
    }
}
