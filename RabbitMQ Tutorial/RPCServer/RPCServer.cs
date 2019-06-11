﻿namespace RPCServer
{
    /// <summary>
    /// Recieves jobs from the rpc_queue
    /// </summary>
    class RPCServer
    {
        static void Main(string[] args)
        {
            var rpcReciever = new RPCServerReceiver();
            rpcReciever.EnableLogging();
            rpcReciever.Initialize("rpc_queue", false, false, false);
            
        }
    }
}
