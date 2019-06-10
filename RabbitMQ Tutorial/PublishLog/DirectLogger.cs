using RabbitMQ.Client;
using Send;
using System;
using Util;

namespace PublishLog
{
    public class DirectLogger : LoggerBase
    {
        public DirectLogger() : base()
        {
            
        }

        public override void Initialize(string logType)
        {
            //Initialize Sender
            _sender = new Sender(_connection, _channel);

            //Initialize exchange
            _channel.ExchangeDeclare("direct_logs", "direct");

            //Get severity and message
            switch (logType)
            {
                case LogTypes.Info:
                    logType = LogTypes.Info;
                    break;
                case LogTypes.Warning:
                    logType = LogTypes.Warning;
                    break;
                case LogTypes.Error:
                    logType = LogTypes.Error;
                    break;
                default:
                    logType = LogTypes.Info;
                    break;
            }

            _sender._exchange = "direct_logs";
            _sender.Initialize(logType);
        }
    }
}
