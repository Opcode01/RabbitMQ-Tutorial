using System;
using System.Text;
using PublishLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Recieve;

namespace RPCServer
{
    /// <summary>
    /// Overrides the basic reciever to send response messages back to the original caller
    /// </summary>
    public class RPCServerReciever : Reciever
    {
        private ILogger _logger;

        public void EnableLogging()
        {
            _logger = new TopicLogger();
            _logger.Initialize("RPCServer.info");
            _logger.SetSilent(true);
        }

        public override void OnMessageRecieved(object sender, BasicDeliverEventArgs e)
        {
            string response = null;

            var body = e.Body;
            var properties = e.BasicProperties;
            var replyProperties = _channel.CreateBasicProperties();
            replyProperties.CorrelationId = properties.CorrelationId;

            try
            {
                //Recieve fibonacci job, run calculations, prepare response
                var message = Encoding.UTF8.GetString(body);
                long n = long.Parse(message);
                if (!runSilent)
                    Console.WriteLine(" [.] fib({0})", message);
                _logger.PublishMessage($"[{this.ToString()}] recieved call for fib({message})", $"{this.ToString()}.{Util.LogTypes.Info}");
                response = fib(n).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" [.] " + ex.Message);
                _logger.PublishMessage($"[{this.ToString()}] encountered an error {ex.Message}", $"{this.ToString()}.{Util.LogTypes.Error}");
                response = "";
            }
            finally
            {
                if(!runSilent)
                    Console.WriteLine(" [.] Done!");
                _logger.PublishMessage($"[{this.ToString()}] has finished.", $"{this.ToString()}.{Util.LogTypes.Info}");
                var responseBytes = Encoding.UTF8.GetBytes(response);
                _channel.BasicPublish("", properties.ReplyTo, replyProperties, responseBytes);
                _channel.BasicAck(e.DeliveryTag, false);
            }
        }
  
        /// <summary>
        /// Represnets a job/unit of work that the reciever does and sends the result back
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static long fib(long n)
        {
            if (n == 0 || n == 1) return n;
            return fib(n - 1) + fib(n - 2);
        }
    }
}
