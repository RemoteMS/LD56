using System;
using EventBus.Events;
using EventBus.Events.UI;
using EventBus.Events.UI.AudioSettings;
using GenericEventBus;
using Helpers.Interfaces;
using Inputs;
using Player;
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
        private PlayerSettings _playerSettings;

        public AppController()
        {
        }

        private void _init()
        {
            _inputReader = SL.Current.Get<InputReader>();
            _playerSettings = SL.Current.Get<PlayerSettings>();
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

        public void Dispose()
        {
        }

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
            _eventBus.SubscribeTo<MasterEvent>(HandleMasterEvent);
            _eventBus.SubscribeTo<AmbientEvent>(HandleAmbientEvent);
            _eventBus.SubscribeTo<SFXEvent>(HandleSoundEvent);
            _eventBus.SubscribeTo<MouseSensitivityEvent>(HandleMouseSensitivity);
        }

        public void UnsubscribeFromEvents()
        {
            _eventBus.UnsubscribeFrom<PauseEvent>(HandlePauseEvent);
            _eventBus.UnsubscribeFrom<ResumeEvent>(HandleResumeEvent);
            _eventBus.UnsubscribeFrom<MasterEvent>(HandleMasterEvent);
            _eventBus.UnsubscribeFrom<AmbientEvent>(HandleAmbientEvent);
            _eventBus.UnsubscribeFrom<SFXEvent>(HandleSoundEvent);
            _eventBus.UnsubscribeFrom<MouseSensitivityEvent>(HandleMouseSensitivity);
        }

        private void HandleSoundEvent(ref SFXEvent eventdata)
        {
        }

        private void HandleAmbientEvent(ref AmbientEvent eventdata)
        {
        }

        private void HandleMasterEvent(ref MasterEvent eventdata)
        {
        }

        private void HandleMouseSensitivity(ref MouseSensitivityEvent eventdata)
        {
            Debug.Log($"eventdata.MouseSensitivity - {eventdata.Value}");
            _playerSettings.mouseSensitivity = eventdata.Value;
        }

    }
}