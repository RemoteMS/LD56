using EventBus.Events;

namespace Common.EventBus.Events.InGame
{
    public class TakenEvent : TBaseEvent
    {
        public int Value;
    }
}