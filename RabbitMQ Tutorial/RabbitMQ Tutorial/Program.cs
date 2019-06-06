using Send;
using Recieve;
using RabbitMQ.Client;

namespace RabbitMQ_Tutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            var sender = new Sender();
            sender.Initialize("task_queue");
            sender._properties.Persistent = true;

            var worker = new Worker();
            worker.Initialize("task_queue");

            sender.SendMessage(GetMessage(args));
        }

        
    }
}
