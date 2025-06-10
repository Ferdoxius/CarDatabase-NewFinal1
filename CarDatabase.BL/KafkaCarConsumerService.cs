using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CarDatabase.Models;
using CarDatabase.BL;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CarDatabase.BL.Kafka
{
    public class KafkaCarConsumerService : BackgroundService
    {
        private readonly ILogger<KafkaCarConsumerService> _logger;
        private readonly ICarService _carService;
        private readonly ConcurrentBag<Car> _cachedCars = new();

        public KafkaCarConsumerService(ILogger<KafkaCarConsumerService> logger, ICarService carService)
        {
            _logger = logger;
            _carService = carService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "car-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("cars-cache");

            _logger.LogInformation("✅ Kafka consumer стартиран и слуша 'cars-cache'...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    var car = JsonSerializer.Deserialize<Car>(result.Message.Value);

                    if (car is not null)
                    {
                        _cachedCars.Add(car);
                        await _carService.CreateAsync(car);
                        _logger.LogInformation("🚗 Добавен автомобил в MongoDB: {Brand} {Model}", car.Brand, car.Model);
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Грешка при обработка на Kafka съобщение");
                }
            }

            consumer.Close();
        }

        public Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            return ExecuteAsync(cancellationToken);
        }

        public IEnumerable<Car> GetCachedCars()
        {
            return _cachedCars;
        }
    }
}
