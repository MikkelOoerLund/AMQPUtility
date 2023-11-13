using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AMQPUtility.RabbitMQ
{
    public class RabbitMqConsumerFactory
    {
        public EventingBasicConsumer CreateByteConsumer(IModel channel, Action<byte[]> onReceive)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (obj, eventArgs) =>
            {
                var bytes = eventArgs.Body;
                onReceive?.Invoke(bytes);
            };

            return consumer;
        }
    }
}