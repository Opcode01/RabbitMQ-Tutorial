using System;
using Send;
using RabbitMQ.Client;

namespace PublishLog
{
    class TopicLogger : LoggerBase
    {

        public TopicLogger() : base()
        {

        }

        public void Initialize(string routingKey)
        {
            //Initialize Sender
            _sender = new Sender(_connection, _channel);
            
            //Initialize exchange
            _channel.ExchangeDeclare("topic_logs", "topic");

            _sender._exchange = "topic_logs";
            _sender.Initialize(routingKey);
        }

        /// <summary>
        /// Args[0] is the severity, args [1] is the message
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (!TestArgs(args))
                return;

            var logPublisher = new TopicLogger();
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
                Console.WriteLine("Cmd line usage: publishlog routingKey message");
                Console.WriteLine("Routing Keys should be of the format: <source>.<severity>");
                return false;
            }
        }

    }
}
