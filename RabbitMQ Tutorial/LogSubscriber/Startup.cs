using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using Recieve;

namespace LogSubscriber
{
    class Startup
    {
        static void Main(string[] args)
        {
        
            if (args.Length <= 0)
            {
                Console.WriteLine("Please specity parameters.");
                return;
            }
            Console.WriteLine("Type 'END' and press [ENTER] to exit.");

            List<ILogSubscriber> subscribers = new List<ILogSubscriber>();

            foreach (var param in args)
            {
                if (param == "info")
                {
                    var infoSubscriber = new InfoSubscriber();
                    infoSubscriber.Initialize("direct_logs", "direct");
                    infoSubscriber.BindQueue("direct_logs");
                    subscribers.Add(infoSubscriber);
                }
                else if (param == "warning")
                {
                    var warningSubscriber = new WarningSubscriber();
                    warningSubscriber.Initialize("direct_logs", "direct");
                    warningSubscriber.BindQueue("direct_logs");
                    subscribers.Add(warningSubscriber);
                }
                else if (param == "error")
                {
                    var errorSubscriber = new ErrorSubscriber();
                    errorSubscriber.Initialize("direct_logs", "direct");
                    errorSubscriber.BindQueue("direct_logs");
                    subscribers.Add(errorSubscriber);
                }
                else
                {
                    /*
                    Console.WriteLine("Command parameter not recognized. Usage - ");
                    Console.WriteLine("\t info - starts an info log listener");
                    Console.WriteLine("\t warning - starts a warning log listener");
                    Console.WriteLine("\t error - starts an error log listener");
                    */
                    var topicSubscriber = new TopicSubscriber();
                    topicSubscriber.Initialize("topic_logs", "topic");
                    topicSubscriber.BindQueue("topic_logs", param);
                    subscribers.Add(topicSubscriber);
                }
            }

            //Wait for user to end
            string ui = "";
            while (ui != "END")
            {
                ui = Console.ReadLine();
            }

            //Close all connections
            foreach(var sub in subscribers)
            {
                sub.CloseConnection();
            }
            return;
        }
    }
}
