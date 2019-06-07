using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Recieve;

namespace LogSubscriber
{
    class LogSubscriber
    {
        static void Main(string[] args)
        {
            //Initialize Connection
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //Initialize exchange
            channel.ExchangeDeclare("logs", "fanout");

            //Initialize Reciever
            Console.WriteLine("Type [END] and press enter to exit.");
            Reciever reciever = new Reciever(connection, channel);
            reciever.Initialize();

            //Bind queue to exchange
            channel.QueueBind(reciever._queue, "logs", "");

        }

    }
}
