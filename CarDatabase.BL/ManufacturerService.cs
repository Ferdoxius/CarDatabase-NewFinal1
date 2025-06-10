using System.Collections.Generic;
using System.Threading.Tasks;
using CarDatabase.Models;
using CarDatabase.DL;

namespace CarDatabase.BL
{
    public interface IManufacturerService
    {
        Task<IEnumerable<Manufacturer>> GetAllAsync();
        Task<Manufacturer?> GetByIdAsync(string id);
        Task CreateAsync(Manufacturer manufacturer);
        Task<bool> UpdateAsync(Manufacturer manufacturer);
        Task<bool> DeleteAsync(string id);
    }

    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<IEnumerable<Manufacturer>> GetAllAsync() =>
            await _manufacturerRepository.GetAllAsync();

        public async Task<Manufacturer?> GetByIdAsync(string id) =>
            await _manufacturerRepository.GetByIdAsync(id);

        public async Task CreateAsync(Manufacturer manufacturer) =>
            await _manufacturerRepository.CreateAsync(manufacturer);

        public async Task<bool> UpdateAsync(Manufacturer manufacturer) =>
            await _manufacturerRepository.UpdateAsync(manufacturer);

        public async Task<bool> DeleteAsync(string id) =>
            await _manufacturerRepository.DeleteAsync(id);
    }
}
