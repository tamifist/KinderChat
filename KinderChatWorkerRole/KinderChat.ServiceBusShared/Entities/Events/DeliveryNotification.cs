using System;

namespace KinderChat.ServiceBusShared.Entities
{
    [Serializable]
    public class DeliveryNotification : Event
    {
        public Guid MessageToken { get; set; }
    }
}