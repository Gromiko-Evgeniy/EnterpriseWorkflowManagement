namespace HiringService.Application.Exceptions.Kafka;

public class KafkaMessageUnsupportedTypeException : CustomException
{
    private const string _messageText = "Kafka message contains unsupported type";

    public KafkaMessageUnsupportedTypeException() : base(_messageText) { }

    public KafkaMessageUnsupportedTypeException(string message) : base(_messageText + " (" + message + ")") { }
}