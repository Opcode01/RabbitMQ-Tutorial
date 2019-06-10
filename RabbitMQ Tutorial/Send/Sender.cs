using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using Util;

namespace Send
{
    public class Sender
    {
        public string _hostname { get; set; } = "localhost";
        public string _exchange { get; set; } = "";
        public IBasicProperties _properties { get; protected set; }
        public string _routingKey { get; private set; }
        private IConnection _connection;
        private IModel _channel;

        /// <summary>
        /// Default constructor - creates a new connection by default
        /// </summary>
        public Sender()
        {
            //Create a connection to the server
            var factory = new ConnectionFactory();
            factory.HostName = this._hostname;
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        /// <summary>
        /// Constructs the class with an already existing connection and model
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="model"></param>
        public Sender(IConnection connection, IModel model)
        {
            _connection = connection;
            _channel = model;
        }

        /// <summary>
        /// Initializes the sender - uses default exchange with non-durable, exclusive, auto-deleting queue if no parameters are specified
        /// </summary>
        public void Initialize(
            string routingKey = "",
            bool durable = false,
            bool exclusive = true,
            bool autoDelete = true,
            IDictionary<string, object> args = null)
        {
            if(_properties == null)
                _properties = _channel.CreateBasicProperties();
               
            //Initialize Queue
            if (routingKey == "")
            {
                _routingKey = _channel.QueueDeclare(routingKey, durable, exclusive, autoDelete, args).QueueName; //Save the name of auto-generated queue
            }
            else
            {
                _channel.QueueDeclare(routingKey, durable, exclusive, autoDelete, args);
                _routingKey = routingKey;
            }
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string msg)
        {
            var body = Encoding.UTF8.GetBytes(msg); //Message Payload

            _channel.BasicPublish(_exchange, _routingKey, _properties, body); //Specify an exchange, or use an empty string for none. Use the name of the queue as the routing key.

            Console.WriteLine(" [{0}] Sent : {1}", _routingKey, msg);
        }

        public void CloseConnection()
        {
            _channel.Close();
            _connection.Close();
            Console.WriteLine("Connection Closed.");
        }

        public static void Main(string[] args)
        {
            if (!TestArgs(args))
                return;

            Sender sender = new Sender();
            sender.Initialize(args[0], true, false, false);
            sender.SendMessage(args[1]);
            sender.CloseConnection(); 
        }

        private static bool TestArgs(string[] args)
        {
            try
            {
                var t1 = args[0];
                var t2 = args[1];
                return true;
            }
            catch (System.IndexOutOfRangeException)
            {
                Console.WriteLine("Cmd line usage: send queueName message");
                return false;
            }
        }
    }
}
