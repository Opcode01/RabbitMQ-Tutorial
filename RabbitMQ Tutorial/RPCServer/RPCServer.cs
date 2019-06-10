using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCServer
{
    /// <summary>
    /// Recieves jobs from the rpc_queue
    /// </summary>
    class RPCServer
    {
        static void Main(string[] args)
        {
            var rpcReciever = new RPCServerReciever();
            rpcReciever.Initialize("rpc_queue", false, false, false);
        }
    }
}
