using System;
using System.Text;
using RabbitMQ.Client.Events;
using System.Threading;

namespace Recieve
{
    public class Worker : Reciever
    {
        public override void OnMessageRecieved(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [y] Recieved {0}", message);

            int dots = message.Split('.').Length - 1;
            DoWork(dots);
            _channel.BasicAck(e.DeliveryTag, false);
        }

        private void DoWork(int work)
        {
            Console.WriteLine("Doing {0} units of work", work);
            Thread.Sleep(work * 1000);
            Console.WriteLine(" [y] Done!");
        }

        static void Main(string[] args)
        {
            Worker worker = new Worker();
            Console.WriteLine("Type [END] and press enter to exit.");
            worker.Initialize("task_queue", true, false, false);

            //Console.ReadLine();
        }

    }
}
