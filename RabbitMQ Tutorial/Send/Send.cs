using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Send
{
    class Send
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
                    channel.QueueDeclare("hello", false, false, false, null);

                    //This section should be refactored into its own method later
                    string message = "Hello, World!";
                    var body = Encoding.UTF8.GetBytes(message); //Message Payload

                    channel.BasicPublish(string.Empty, "hello", null, body); //Specify an exchange, or use an empty string for none. Use the name of the queue as the routing key.

                    Console.WriteLine(" [x] Sent {0}", message);   
                }
            }
        }

        public static void Main(string[] args)
        {
            Send sender = new Send();
            sender.Initialize();

            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}
