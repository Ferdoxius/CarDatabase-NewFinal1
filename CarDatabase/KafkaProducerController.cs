using Microsoft.AspNetCore.Mvc;
using CarDatabase.BL.Kafka;
using CarDatabase.Models;
using System.Threading.Tasks;

namespace CarDatabase.API.Controllers
{
    [ApiController]
    [Route("api/KafkaCar")]
    public class KafkaProducerController : ControllerBase
    {
        private readonly KafkaCarProducerService _producer;

        public KafkaProducerController(KafkaCarProducerService producer)
        {
            _producer = producer;
        }

        [HttpPost("send-car")]
        public async Task<IActionResult> SendCar([FromBody] Car car)
        {
            await _producer.SendCarAsync(car);
            return Ok("Car sent to Kafka.");
        }
    }
}
