using Confluent.Kafka;
using Newtonsoft.Json;

namespace IdentityService.Application.Services.Kafka;

public static class KafkaProducerBaseFunctionality
{
    public static void SendMessage<T>(string bootstrapServers, string topic, T data)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };

        using (var producer = new ProducerBuilder<string, string>(config).Build())
        {
            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonConvert.SerializeObject(data)
            };

            producer.Produce(topic, message);
            producer.Flush(TimeSpan.FromSeconds(10));
        }
    }
}
