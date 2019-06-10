using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSubscriber
{
    class TopicSubscriber : LogSubscriberBase
    {
        public override void BindQueue(string newExchange, string bindingKey)
        {
            //Bind queue to exchange
            _exchange_name = newExchange;
            _reciever.log_type = bindingKey;
            _channel.QueueBind(_reciever._queue, _exchange_name, bindingKey);
        }
    }
}
