using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receive
{
    public class LogReceiver : Receiver
    {
        public string log_type { get; set; }

        public LogReceiver(IConnection connection, IModel model)
        {
            _connection = connection;
            _channel = model;
        }

        public override void OnMessageRecieved(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [{0}] : {1}", log_type, message);
            _channel.BasicAck(e.DeliveryTag, false);
        }
    }
}
