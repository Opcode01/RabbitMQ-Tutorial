using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishLog
{
    public interface ILogger
    {
        void PublishMessage(string message, string severity);

        void CloseConnection();

        void Initialize(string routingKey);

        /// <summary>
        /// If set to true, instructs the logger to not print out any of its own messages to the console,
        /// only the messages it recieves
        /// </summary>
        /// <param name="s"></param>
        void SetSilent(bool s);

    }
}
