using RabbitMQ.Client;
using Send;
using System;
using Util;

namespace PublishLog
{
    class PublishLog
    {
        /// <summary>
        /// Args[0] is the severity, args [1] is the message
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (!TestArgs(args))
                return;

            //Initialize Connection
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //Initialize exchange
            channel.ExchangeDeclare("direct_logs", "direct");

            //Get severity and message
            var severity = args[0];
            switch (severity)
            {
                case LogTypes.Info:
                    severity = LogTypes.Info;
                    break;
                case LogTypes.Warning:
                    severity = LogTypes.Warning;
                    break;
                case LogTypes.Error:
                    severity = LogTypes.Error;
                    break;
                default:
                    severity = LogTypes.Info;
                    break;
            }
            var message = args[1];

            //Initialize Sender
            Sender sender = new Sender(connection, channel);
            sender._exchange = "direct_logs";
            sender.Initialize(severity);

            //Send message
            sender.SendMessage(args[1]);

            //Close connection
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
                Console.WriteLine("Cmd line usage: publishlog messageType message");
                Console.WriteLine("Valid message types are: 'info', 'warning', 'error' ");
                return false;
            }
        }

    }

}
