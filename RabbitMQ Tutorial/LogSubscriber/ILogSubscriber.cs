using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSubscriber
{
    interface ILogSubscriber
    {
        void Initialize(string exchange_name, string exchange_type);
        void BindQueue(string newExchange);
        void CloseConnection();
    }
}
