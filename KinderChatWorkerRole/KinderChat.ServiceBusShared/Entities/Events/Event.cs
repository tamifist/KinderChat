using System;
using System.Runtime.Serialization;

namespace KinderChat.ServiceBusShared.Entities
{
    [Serializable]
    [KnownType(typeof(Message))]
    [KnownType(typeof(DeliveryNotification))]
    [KnownType(typeof(GroupChangedEvent))]
    [KnownType(typeof(IsTypingEvent))]
    [KnownType(typeof(NewDeviceCreatedEvent))]
    [KnownType(typeof(SeenNotification))]
    public abstract class Event
    {
        public Guid EventId { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public string ReceiverDeviceId { get; set; }
    }
}