using RabbitMQ.Client;
using Recieve;

namespace LogSubscriber
{
    class LogSubscriberBase : ILogSubscriber
    {
        protected IModel _channel;

        protected LogReciever _reciever;

        protected string _exchange_name;

        public virtual void BindQueue(string newExchange)
        {
            //Bind queue to exchange
            _exchange_name = newExchange;
            _channel.QueueBind(_reciever._queue, _exchange_name, Util.LogTypes.Info);
        }

        public virtual void BindQueue(string newExchange, string bindingKey) {
            //Method must be overridden by sub class
        }

        public void Initialize(string exchange_name, string exchange_type)
        {
            //Initialize Connection
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _exchange_name = exchange_name;

            //Initialize exchange
            _channel.ExchangeDeclare(_exchange_name, exchange_type);

            //Initialize Reciever
            _reciever = new LogReciever(connection, _channel);
            _reciever.Initialize();

            //BindQueue(_exchange_name);
        }

        public void CloseConnection()
        {
            _reciever.CloseConnection();
        }
    }
}
