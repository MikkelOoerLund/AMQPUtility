

namespace AMQPUtility.RabbitMQ
{
    public class RabbitMqQueue
    {
        public string? Name { get; set; }

        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }

        public IDictionary<string, object>? Arguments { get; set; }

    }
}