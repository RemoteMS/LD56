namespace Helpers.Interfaces
{
    public interface IGameEventListener
    {
        public void SubscribeToEvents();

        public void UnsubscribeFromEvents();
    }
}
