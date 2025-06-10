using System.Collections.Generic;
using System.Threading.Tasks;
using CarDatabase.BL;
using CarDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarDatabase.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetAll() =>
            Ok(await _carService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetById(string id)
        {
            var car = await _carService.GetByIdAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        [HttpGet("brand/{brand}")]
        public async Task<ActionResult<IEnumerable<Car>>> GetByBrand(string brand) =>
            Ok(await _carService.GetCarsByBrandAsync(brand));

        [HttpGet("maxprice/{maxPrice}")]
        public async Task<ActionResult<IEnumerable<Car>>> GetByMaxPrice(decimal maxPrice) =>
            Ok(await _carService.GetCarsByMaxPriceAsync(maxPrice));

        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            await _carService.CreateAsync(car);
            return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Car car)
        {
            if (id != car.Id) return BadRequest();
            var updated = await _carService.UpdateAsync(car);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _carService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
