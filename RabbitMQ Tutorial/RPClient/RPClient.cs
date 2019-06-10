using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Send;
using RabbitMQ.Client;
using System.Collections.Concurrent;
using Recieve;

namespace RPClient
{
    class RPClient
    {
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();

        private Sender _sender;

        private Reciever _reciever;

        /// <summary>
        /// Sends a message and then blocks until it recieves a response
        /// </summary>
        public RPClient()
        {
            //Initialize sender
            _sender = new Sender();
            _sender.Initialize("rpc_queue", false, false, false);

            //Iniitalize reciever
            _reciever = new RPClientReciever(respQueue);
            _reciever.Initialize();

            //Set sender properties
            var props = _sender._properties;
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = _reciever._queue;
            _sender._properties = props;

            //Set reciever properties
            var rProps = _reciever._properties;
            rProps.CorrelationId = correlationId;

        }

        public string Call(string msg)
        {
            _sender.SendMessage(msg);

            //Blocks until something appears in the queue
            return respQueue.Take();
        }

        public void CloseConnection()
        {
            _sender.CloseConnection();
            _reciever.CloseConnection();
        }

    }

    public class Rpc{

        public static void Main(string[] args)
        {
            var rpcClient = new RPClient();

            Console.WriteLine(" [x] Requesting fib(40) ");
            var response = rpcClient.Call("40");

            Console.WriteLine(" [.] Recieved response '{0}'", response);
            rpcClient.CloseConnection();

            Console.ReadLine();
        }
    }
}
