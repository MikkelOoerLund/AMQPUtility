using AMQPUtility.RabbitMQ;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace AMQPUtilityUnitTest
{
    public class RabbitMqChannelTester
    {
        private RabbitMqQueue _queue;
        private ConnectionFactory _connectionFactory;
        private RabbitMqConsumerFactory _consumerFactory;
        private readonly ITestOutputHelper _output;



        public RabbitMqChannelTester(ITestOutputHelper output)
        {
            _output = output;


            _consumerFactory = new RabbitMqConsumerFactory();

            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
            };

            _queue = new RabbitMqQueue()
            {
                Name = "testQueue",
                Durable = false,
                Arguments = null,
                Exclusive = false,
                AutoDelete = false,
            };
        }


        [Fact]
        public void TestDeclareQueue()
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var rabbitChannel = new RabbitMqChannel(channel);
                rabbitChannel.DeclareQueue(_queue);

                var isQueueDeclared = channel.QueueDeclarePassive(_queue.Name);
            }
        }

        [Fact] 
        public void TestPublish()
        {

            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var rabbitChannel = new RabbitMqChannel(channel);

                var isQueueDeclared = channel.QueueDeclarePassive(_queue.Name);

                var message = "TestPublishMessage";
                var bytesToSend = Encoding.UTF8.GetBytes(message);


                var package = new RabbitMqPackage()
                {
                    RoutingKey = _queue.Name,
                    Exchange = string.Empty,
                    Body = bytesToSend,
                };


                rabbitChannel.Publish(package);
            }
        }


        [Fact]
        public void TestConsume()
        {

            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var rabbitChannel = new RabbitMqChannel(channel);

                var isQueueDeclared = channel.QueueDeclarePassive(_queue.Name);

                if (isQueueDeclared.MessageCount == 0)
                {
                    throw new Exception("No message to consume");
                }




                var consumer = _consumerFactory.CreateByteConsumer(channel, (bytes) =>
                {
                    var message = Encoding.UTF8.GetString(bytes);
                    _output.WriteLine($"Received message: {message}");
                });



                rabbitChannel.Consume(_queue, consumer);

                Thread.Sleep(500);
            }
        }



    }
}