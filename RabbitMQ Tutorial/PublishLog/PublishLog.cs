using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Send;

namespace PublishLog
{
    class PublishLog
    {
        static void Main(string[] args)
        {
            //Initialize Connection
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //Initialize exchange
            channel.ExchangeDeclare("logs", "fanout");

            //Initialize Sender
            Sender sender = new Sender(connection, channel);
            sender._exchange = "logs";
            sender.Initialize();

            //Send message
            sender.SendMessage(ParseMessage(args));

            //Close connection
            sender.CloseConnection();
           
        }

        private static string ParseMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}
