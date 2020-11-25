using Configuration;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace Send
{
    public class Send
    {

        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                channel.QueueDeclare(queue: QueueSettings.Queuename,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var queueSettings = channel.CreateBasicProperties();

                for (long i = 0; i < 1000000; i++)
                {
                    var sparePartMessage = new QueueMessage<SparePart>
                    {
                        Key = Guid.NewGuid(),

                        Value = new SparePart
                        {
                            Name = "Ersatzteil",
                            Category = "Bremsen",
                            Manufacturer = "Brembo",
                            PartNumber = "0815",
                            Price = 39.99M
                        }
                    };

                    queueSettings.MessageId = sparePartMessage.Key.ToString();

                    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sparePartMessage));

                    channel.BasicPublish(exchange: "",
                                         routingKey: QueueSettings.Queuename,
                                         basicProperties: queueSettings,
                                         body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sparePartMessage)));
                }

                Console.WriteLine(" [x] Sent {0}", "Spareparts");

                //string message = $"Hello World-{i}!";

                //var body = Encoding.UTF8.GetBytes(message);

                //channel.BasicPublish(exchange: "",
                //                     routingKey: QueueSettings.Queuename,
                //                     basicProperties: null,
                //                     body: body);

                //Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
