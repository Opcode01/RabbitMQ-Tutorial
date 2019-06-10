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
        private ILogger _loggerInfo;

        private ILogger _loggerError;

        public void EnableLogging()
        {
            _loggerInfo = new TopicLogger();
            _loggerInfo.Initialize("RPCServer.info");
            _loggerInfo.SetSilent(true);

            _loggerError = new TopicLogger();
            _loggerError.Initialize("RPCServer.Error");
            _loggerError.SetSilent(true);
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
                _loggerInfo.PublishMessage($"[{this.ToString()}.INFO] was called with fib({message})");
                response = fib(n).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" [.] " + ex.Message);
                _loggerError.PublishMessage($"[{this.ToString()}.ERROR] encountered an error\n {ex.Message}");
                response = "";
            }
            finally
            {
                Console.WriteLine(" [.] Done!");
                _loggerInfo.PublishMessage($"[{this.ToString()}.INFO] has finished.");
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
