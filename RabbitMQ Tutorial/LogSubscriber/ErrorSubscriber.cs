using RabbitMQ.Client;

namespace LogSubscriber
{
    class ErrorSubscriber : LogSubscriberBase
    {
        public override void BindQueue(string newExchange)
        {
            _exchange_name = newExchange;
            _channel.QueueBind(_reciever._queue, newExchange, Util.LogTypes.Error);
        }

    }
}
