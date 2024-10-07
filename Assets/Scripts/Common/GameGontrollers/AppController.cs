using System;
using EventBus.Events;
using GenericEventBus;
using Helpers.Interfaces;
using Inputs;
using ServiceLocator;
using UnityEngine;


namespace Common.GameGontrollers
{
    public class AppController
        : IService,
            IInitializable,
            IBeforeDisposable,
            IDisposable,
            IGameEventListener
    {
        private InputReader _inputReader;
        private GenericEventBus<TBaseEvent> _eventBus;

        public AppController() { }

        private void _init()
        {
            _inputReader = SL.Current.Get<InputReader>();
        }

        public void Init()
        {
            _eventBus = SL.Current.Get<GenericEventBus<TBaseEvent>>();
            _init();

            SubscribeToEvents();
            UnityEngine.Cursor.visible = false;
        }

        public void BeforeDispose()
        {
            UnsubscribeFromEvents();
        }

        public void Dispose() { }

        private void HandlePauseEvent(ref PauseEvent eventData)
        {
            Time.timeScale = 0f;
            _inputReader.SetUI();
            UnityEngine.Cursor.visible = true;
        }

        private void HandleResumeEvent(ref ResumeEvent eventData)
        {
            Time.timeScale = 1f;
            _inputReader.SetGameplay();
            UnityEngine.Cursor.visible = false;
        }

        public void SubscribeToEvents()
        {
            _eventBus.SubscribeTo<PauseEvent>(HandlePauseEvent);
            _eventBus.SubscribeTo<ResumeEvent>(HandleResumeEvent);
        }

        public void UnsubscribeFromEvents()
        {
            _eventBus.UnsubscribeFrom<PauseEvent>(HandlePauseEvent);
            _eventBus.UnsubscribeFrom<ResumeEvent>(HandleResumeEvent);
        }
    }
}