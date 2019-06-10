using System;
using Send;
using RabbitMQ.Client;

namespace PublishLog
{
    class TopicLogger : LoggerBase
    {

        public TopicLogger() : base()
        {

        }

        public void Initialize(string routingKey)
        {
            //Initialize Sender
            _sender = new Sender(_connection, _channel);
            
            //Initialize exchange
            _channel.ExchangeDeclare("topic_logs", "topic");

            _sender._exchange = "topic_logs";
            _sender.Initialize(routingKey);
        }

    }
}
