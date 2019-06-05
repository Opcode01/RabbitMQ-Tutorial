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

        private string _exchange = "";

        private string _queue = "";

        private IConnection _connection;

        private IModel _channel;

        public void Initialize(string queueName)
        {
            //Create a connection to the server
            var factory = new ConnectionFactory();
            factory.HostName = this._hostname;
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queue = queueName;

            //Initialize queue
            _channel.QueueDeclare(queueName, false, false, false, null);
        }

        public void SendMessage(string msg)
        {
            var body = Encoding.UTF8.GetBytes(msg); //Message Payload

            _channel.BasicPublish(_exchange, _queue, null, body); //Specify an exchange, or use an empty string for none. Use the name of the queue as the routing key.

            Console.WriteLine(" [x] Sent {0}", msg);
        }

        public void CloseConnection()
        {
            _channel.Close();
            _connection.Close();
        }

        public static void Main(string[] args)
        {
            Send sender = new Send();
            sender.Initialize("hello");
            Console.WriteLine("Press [Enter] to send hello, or type 'END' to exit");
            string input = "";
            do
            {
                sender.SendMessage("Hello, World!");
                input = Console.ReadLine();

            } while (input != "END");
            sender.CloseConnection();
        }
    }
}
