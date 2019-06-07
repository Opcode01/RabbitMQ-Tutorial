using RabbitMQ.Client;
using Send;
using System;
using Util;

namespace PublishLog
{
    public class Logger
    {
        public string _hostname { get; set; } = "localhost";
        private Sender sender;

        public Logger()
        {

        }

        public void Initialize(string logType)
        {
            //Initialize Connection
            var factory = new ConnectionFactory();
            factory.HostName = _hostname;
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //Initialize exchange
            channel.ExchangeDeclare("direct_logs", "direct");

            //Get severity and message
            switch (logType)
            {
                case LogTypes.Info:
                    logType = LogTypes.Info;
                    break;
                case LogTypes.Warning:
                    logType = LogTypes.Warning;
                    break;
                case LogTypes.Error:
                    logType = LogTypes.Error;
                    break;
                default:
                    logType = LogTypes.Info;
                    break;
            }

            //Initialize Sender
            sender = new Sender(connection, channel);
            sender._exchange = "direct_logs";
            sender.Initialize(logType);
        }

        public void PublishMessage(string message)
        {
            sender.SendMessage(message);
        }

        public void CloseConnection()
        {
            sender.CloseConnection();
        }

        /// <summary>
        /// Args[0] is the severity, args [1] is the message
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (!TestArgs(args))
                return;

            var logPublisher = new Logger();
            logPublisher.Initialize(args[0]);
            var message = args[1];

            //Send message
            logPublisher.PublishMessage(args[1]);

            //Console.ReadLine();

            //Close connection
            logPublisher.CloseConnection();
           
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
