using System;

namespace LogSubscriber
{
    class InfoSubscriber : LogSubscriberBase
    {
        public override void BindQueue(string newExchange)
        {
            _reciever.log_type = Util.LogTypes.Info;
            base.BindQueue(newExchange);
        }
    }
}
