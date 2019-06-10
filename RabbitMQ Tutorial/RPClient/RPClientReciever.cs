using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recieve;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;

namespace RPClient
{
    public class RPClientReciever : Reciever
    {
        private BlockingCollection<string> _responseQueue;

        public RPClientReciever(BlockingCollection<string> responseQueue)
        {
            _responseQueue = responseQueue;
        }

        public override void OnMessageRecieved(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var response = Encoding.UTF8.GetString(body);

            //If the correlation ID of the properties we recieved is the same as our own -
            if(e.BasicProperties.CorrelationId == _properties.CorrelationId)
            {
                _responseQueue.Add(response);
            }
        }
    }
}
