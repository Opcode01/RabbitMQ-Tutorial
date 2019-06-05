using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Recieve
{
    class Recieve
    {
        private string _hostname { get; set; } = "localhost";
        public void Initialize()
        {
            //Create a connection to the server
            var factory = new ConnectionFactory();
            factory.HostName = this._hostname;

            //Using statement will autodispose of the connection and channel when they are finished
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //Note, if a queue with these settings is already declared, RabbitMQ will not create a new one
                    channel.QueueDeclare("hello", false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);

                    //Create a callback function to execute when a message is recieved
                    consumer.Received += OnMessageRecieved;

                    string ui = "";
                    Console.WriteLine("Waiting for messages...");
                    do
                    {
                        channel.BasicConsume("hello", true, consumer);
                        ui = Console.ReadLine();

                    } while (ui != "END");
                }
            }
        }

        private void OnMessageRecieved(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [y] Recieved {0}", message);
        }

        static void Main(string[] args)
        {
            Recieve reciever = new Recieve();
            Console.WriteLine("Type [END] and press enter to exit.");
            reciever.Initialize();
        }
    }
}
