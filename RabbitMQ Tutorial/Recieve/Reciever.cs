using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Recieve
{
    public class Reciever
    {
        private string _hostname { get; set; } = "localhost";

        private string _queue = "";

        private string _input = "";

        private IConnection _connection;

        private IModel _channel;

        public void Initialize(string queueName)
        {
            //Create a connection to the server
            var factory = new ConnectionFactory();
            factory.HostName = this._hostname;
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            //Note, if a queue with these settings is already declared, RabbitMQ will not create a new one
            _channel.QueueDeclare(queueName, false, false, false, null);

            var consumer = new EventingBasicConsumer(_channel);

            //Create a callback function to execute when a message is recieved
            consumer.Received += OnMessageRecieved;
            Console.WriteLine("Waiting for messages...");
            BeginLoop(consumer);
           
        }

        public void CloseConnection()
        {
            _input = "END";
            _channel.Close();
            _connection.Close();
            Console.WriteLine("Connection closed.");
        }

        private void BeginLoop(EventingBasicConsumer consumer)
        {
            do
            {
                _channel.BasicConsume(_queue, true, consumer);
                _input = Console.ReadLine();

            } while (_input != "END");
            CloseConnection();
        }

        public virtual void OnMessageRecieved(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [y] Recieved {0}", message);
        }

        static void Main(string[] args)
        {
            Reciever reciever = new Reciever();
            Console.WriteLine("Type [END] and press enter to exit.");
            reciever.Initialize("hello");

            Console.ReadLine();
        }
        
    }
        
}
