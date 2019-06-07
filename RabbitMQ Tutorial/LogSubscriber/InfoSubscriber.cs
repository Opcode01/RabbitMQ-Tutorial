using System;

namespace LogSubscriber
{
    class InfoSubscriber : LogSubscriberBase
    {
        public override void BindQueue(string newExchange)
        {
            base.BindQueue(newExchange);
        }
    }
}
