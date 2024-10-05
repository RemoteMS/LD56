using UnityEngine;

namespace EventBus.Events
{
    public struct LookEvent : TBaseEvent
    {
        public Vector2 LookDirection;
    }
}