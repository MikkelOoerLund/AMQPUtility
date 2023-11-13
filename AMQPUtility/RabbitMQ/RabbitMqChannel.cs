
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace AMQPUtility.RabbitMQ
{
    public class RabbitMqChannel
    {
        private IModel _channel;

        public RabbitMqChannel(IModel channel)
        {
            _channel = channel;
        }


        public void DeclareQueue(RabbitMqQueue queue)
        {
            _channel.QueueDeclare(
                queue: queue.Name,
                durable: queue.Durable,
                arguments: queue.Arguments,
                exclusive: queue.Exclusive,
                autoDelete: queue.AutoDelete
            );
        }


        public void Consume(RabbitMqQueue queue, EventingBasicConsumer? onConsume)
        {
            _channel.BasicConsume(
                queue: queue.Name, 
                autoAck: true, 
                consumer: onConsume);
        }

        public void Publish(RabbitMqPackage package)
        {
            _channel.BasicPublish(
                package.Exchange,
                package.RoutingKey,
                package.Mandatory,
                package.BasicProperties,
                package.Body);
        }
    }
}