using Microsoft.AspNetCore.Mvc;
using CarDatabase.BL.Kafka;

namespace CarDatabase.API.Controllers
{
    [ApiController]
    [Route("api/KafkaCar")]
    public class KafkaConsumerController : ControllerBase
    {
        private readonly KafkaCarConsumerService _consumer;

        public KafkaConsumerController(KafkaCarConsumerService consumer)
        {
            _consumer = consumer;
        }

        [HttpGet("cached-cars")]
        public IActionResult GetCachedCars()
        {
            var cars = _consumer.GetCachedCars();
            return Ok(cars);
        }
    }
}
