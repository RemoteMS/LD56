using UnityEngine;

namespace EventBus.Events
{
    public struct MoveEvent : TBaseEvent
    {
        public Vector2 Direction { get; set; }
    }
}
