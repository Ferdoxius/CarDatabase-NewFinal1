using System.Text.Json;
using CarDatabase.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CarDatabase.BL.Kafka
{
    public class KafkaCarProducerService
    {
        private readonly ILogger<KafkaCarProducerService> _logger;

        public KafkaCarProducerService(ILogger<KafkaCarProducerService> logger)
        {
            _logger = logger;
        }

        public async Task SendCarAsync(Car car)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var value = JsonSerializer.Serialize(car);

            var result = await producer.ProduceAsync("cars-cache", new Message<Null, string> { Value = value });
            _logger.LogInformation("Изпратен автомобил към Kafka: {0}", value);
        }
    }
}
