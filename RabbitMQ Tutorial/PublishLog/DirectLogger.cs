using RabbitMQ.Client;
using Send;
using System;
using Util;

namespace PublishLog
{
    public class DirectLogger : LoggerBase
    {
        public DirectLogger() : base()
        {
            
        }

        public void Initialize(string logType)
        {
            //Initialize Sender
            _sender = new Sender(_connection, _channel);

            //Initialize exchange
            _channel.ExchangeDeclare("direct_logs", "direct");

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

            _sender._exchange = "direct_logs";
            _sender.Initialize(logType);
        }


        /// <summary>
        /// Args[0] is the severity, args [1] is the message
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (!TestArgs(args))
                return;

            var logPublisher = new DirectLogger();
            logPublisher.Initialize(args[0]);
            var message = args[1];

            //Send message
            logPublisher.PublishMessage(message);

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
