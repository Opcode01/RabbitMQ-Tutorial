using System;
using RabbitMQ.Client;
using Recieve;

namespace LogSubscriber
{
    class Startup
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type [END] and press enter to exit.");
            var infoSubscriber = new InfoSubscriber();
            infoSubscriber.Initialize("direct_logs", "direct");

            var warningSubscriber = new WarningSubscriber();
            warningSubscriber.Initialize("direct_logs", "direct");
        }

    }
}
