using Confluent.Kafka;
using Newtonsoft.Json;

namespace HiringService.Application.Kafka;

public static class BaseKafkaConsumerFunctionality
{
    public static async Task StartConsuming<T>(
        CancellationToken stoppingToken, ConsumerConfig config,
        string topic, Func<T, Task> messageValueAsyncProcessing)
    {
        await Task.Run(async () =>
        {
            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    try
                    {
                        Console.WriteLine(consumeResult.Message.Value);
                        var data = JsonConvert.DeserializeObject<T>(consumeResult.Message.Value);

                        if (data is not null)
                        {
                            try
                            {
                                await messageValueAsyncProcessing(data);
                            }
                            catch (Exception ex)
                            {
                                await Console.Out.WriteLineAsync(ex.Message);
                                //incorrect data
                                //throw ?
                                //log ?
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //unsupported type
                    }

                    consumer.Commit(consumeResult);
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        });
    }
}