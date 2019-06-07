using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Recieve
{
    public class Reciever
    {
        private string _hostname { get; set; } = "localhost";

        public string _queue { get; private set; }

        private string _input = "";

        protected IConnection _connection;

        protected IModel _channel;
        public IBasicProperties _properties { get; protected set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Reciever()
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
        public Reciever(IConnection connection, IModel model)
        {
            _connection = connection;
            _channel = model;
        }

        public void Initialize(
           string routingKey = "",
           bool durable = false,
           bool exclusive = true,
           bool autoDelete = true,
           IDictionary<string, object> args = null)
        {
            if(_properties == null)
                _properties = _channel.CreateBasicProperties();

            //Tell RabbitMQ not to give any more than one message to a reciever at a time
            _channel.BasicQos(0, 1, false);

            //Note, if a queue with these settings is already declared, RabbitMQ will not create a new one
            //Initialize Queue
            if (routingKey == "")
            {
                _queue = _channel.QueueDeclare(routingKey, durable, exclusive, autoDelete, args).QueueName; //Save the name of auto-generated queue
            }
            else
            {
                _channel.QueueDeclare(routingKey, durable, exclusive, autoDelete, args);
                _queue = routingKey;
            }

            var consumer = new EventingBasicConsumer(_channel);

            //Create a callback function to execute when a message is recieved
            consumer.Received += OnMessageRecieved;
            Console.WriteLine("Waiting for messages...");

            //Run this in a separate thread so we can later close it out from code (using .NET tasks)
            Task.Factory.StartNew(() => BeginLoop(consumer));
            
        }

        public void CloseConnection()
        {
            if (!_connection.IsOpen)
            {
                return;
            }

            _input = "END";     //Needed to easily break out of the while loop in the case we close this from code
            _channel.Close();
            _connection.Close();
            Console.WriteLine("Connection closed.");
        }

        private void BeginLoop(EventingBasicConsumer consumer)
        {
            do
            {
                _channel.BasicConsume(_queue, false, consumer);
                _input = Console.ReadLine();

            } while (_input != "END");
            CloseConnection();
        }

        public virtual void OnMessageRecieved(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [y] Recieved {0}", message);
            _channel.BasicAck(e.DeliveryTag, false);
        }

        static void Main(string[] args)
        {
            Reciever reciever = new Reciever();
            Console.WriteLine("Type [END] and press enter to exit.");
            reciever.Initialize("hello", false, false, false);

            //Console.ReadLine();
        }
        
    }
        
}
