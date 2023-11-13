
using RabbitMQ.Client;

namespace AMQPUtility.RabbitMQ
{
    public class RabbitMqPackage
    {
        public bool Mandatory { get; set; }
        public byte[]? Body { get; set; }
        public string? Exchange { get; set; }
        public string? RoutingKey { get; set; }
        public IBasicProperties? BasicProperties { get; set; }
    }
}