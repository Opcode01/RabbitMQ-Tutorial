﻿using RabbitMQ.Client;

namespace LogSubscriber
{
    class WarningSubscriber : LogSubscriberBase
    {
        public override void BindQueue(string newExchange)
        {
            _exchange_name = newExchange;
            _reciever.log_type = Util.LogTypes.Warning;
            _channel.QueueBind(_reciever._queue, newExchange, Util.LogTypes.Warning);
        }
    }
}
