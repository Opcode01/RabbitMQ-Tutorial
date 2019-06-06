using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Send
{
    public class Sender
    {
        public string _hostname { get; set; } = "localhost";

        private string _exchange = "";

        private string _queue = "";

        private IConnection _connection;

        private IModel _channel;
        public IBasicProperties _properties { get; set; }

        public void Initialize(string queueName)
        {
            //Create a connection to the server
            var factory = new ConnectionFactory();
            factory.HostName = this._hostname;
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _properties = _channel.CreateBasicProperties();
            _queue = queueName;

            //Initialize queue
            _channel.QueueDeclare(queueName, false, false, false, null);
        }

        public void SendMessage(string msg)
        {
            var body = Encoding.UTF8.GetBytes(msg); //Message Payload

            _channel.BasicPublish(_exchange, _queue, _properties, body); //Specify an exchange, or use an empty string for none. Use the name of the queue as the routing key.

            Console.WriteLine(" [x] Sent {0}", msg);
        }

        public void CloseConnection()
        {
            _channel.Close();
            _connection.Close();
            Console.WriteLine("Connection Closed.");
        }

        private static string ParseMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }

        public static void Main(string[] args)
        {
            Sender sender = new Sender();
            sender.Initialize("task_queue");
            sender.SendMessage(ParseMessage(args));
            
            /*Console.WriteLine("Press [Enter] to send hello, or type 'END' to exit");
            string input = "";
            do
            {
                input = Console.ReadLine();
                sender.SendMessage(input);

            } while (input != "END");
            */
            sender.CloseConnection(); 
        }
    }
}
