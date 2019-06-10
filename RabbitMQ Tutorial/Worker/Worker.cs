using System;
using System.Text;
using RabbitMQ.Client.Events;
using System.Threading;
using PublishLog;

namespace Recieve
{
    public class Worker : Reciever
    {
        private ILogger _logger;

        public override void OnMessageRecieved(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            if(!runSilent)
                Console.WriteLine(" [y] Recieved {0}", message);
            if (_logger != null)
                _logger.PublishMessage($"[{this.ToString()}] Recieved {message}");

            int dots = message.Split('.').Length - 1;
            DoWork(dots);
            _channel.BasicAck(e.DeliveryTag, false);
        }

        private void DoWork(int work)
        {
            if(!runSilent)
                Console.WriteLine("Doing {0} units of work", work);
            if (_logger != null)
                _logger.PublishMessage($"[{this.ToString()}] doing {work} units of work");

            Thread.Sleep(work * 1000);

            if(!runSilent)
                Console.WriteLine(" [y] Done!");
            if (_logger != null)
                _logger.PublishMessage($"[{this.ToString()}] has finished.");
        }

        public void EnableLogging()
        {
            _logger = new TopicLogger();
            _logger.Initialize("worker.info");
            _logger.SetSilent(true);
        }

        static void Main(string[] args)
        {
            Worker worker = new Worker();
            Console.WriteLine("Type [END] and press enter to exit.");
            worker.Initialize("task_queue", true, false, false);
            worker.EnableLogging();

            string ui = "";
            while (ui != "END")
            {
                ui = Console.ReadLine();
            }
            worker._logger.CloseConnection();
            worker.CloseConnection();
        }

    }
}
