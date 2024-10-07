using System;
using EventBus.Events;
using GenericEventBus;
using Helpers.Interfaces;
using ServiceLocator;

namespace UI
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public class UIMenuHandler : MonoBehaviour, IService, IInitializable, IGameEventListener, IDisposable,
        IBeforeDisposable
    {
        [SerializeField] private UIDocument uiDocument;

        private Button _resumeButton;
        private Button _settingsButton;
        private Button _exitButton;

        private Slider[] _sliders;
        private float[] _sliderValues;

        private VisualElement _root;
        private VisualElement _uiMenu;

        [SerializeField] private bool _hidden = true;
        private GenericEventBus<TBaseEvent> _eventBus;

        private const string Hidden = "hidden";

        private void OnEnable()
        {
            _root = uiDocument.rootVisualElement;
            _uiMenu = _root.Q<VisualElement>("ui-menu");

            _resumeButton = _root.Q<Button>("resume-button");
            _settingsButton = _root.Q<Button>("settings-button");
            _exitButton = _root.Q<Button>("exit-button");

            _resumeButton.clicked += OnResumeClicked;
            _settingsButton.clicked += OnSettingsClicked;
            _exitButton.clicked += OnExitClicked;


            _sliders = _root.Query<Slider>().ToList().ToArray();
            _sliderValues = new float[_sliders.Length];


            for (var i = 0; i < _sliders.Length; i++)
            {
                var sliderIndex = i;
                _sliders[i].RegisterValueChangedCallback(evt => OnSliderValueChanged(evt, sliderIndex));
            }
        }

        private void OnDisable()
        {
            _resumeButton.clicked -= OnResumeClicked;
            _settingsButton.clicked -= OnSettingsClicked;
            _exitButton.clicked -= OnExitClicked;

            foreach (var slider in _sliders)
            {
                slider.UnregisterValueChangedCallback(evt => OnSliderValueChanged(evt, 0));
            }

            UnsubscribeFromEvents();
        }

        private void OnResumeClicked()
        {
            _eventBus.Raise<ResumeEvent>(new());
            Debug.Log("Resume button clicked!");
        }

        private void OnSettingsClicked()
        {
            Debug.Log("Settings button clicked!");
        }

        private void OnExitClicked()
        {
            Debug.Log("Exit button clicked!");
        }


        private void OnSliderValueChanged(ChangeEvent<float> evt, int sliderIndex)
        {
            _sliderValues[sliderIndex] = evt.newValue;

            Debug.Log($"Slider {sliderIndex} value changed to: {evt.newValue}");

            HandleSliderData(sliderIndex, evt.newValue);
        }


        private void HandleSliderData(int sliderIndex, float value)
        {
            switch (sliderIndex)
            {
                case 0:
                    Debug.Log($"slider - Adjusting volume to: {value}");
                    break;
                case 1:
                    Debug.Log($"slider - Adjusting brightness to: {value}");
                    break;
                case 2:
                    Debug.Log($"slider - Adjusting some other parameter to: {value}");
                    break;
                default:
                    Debug.Log("Unknown slider index");
                    break;
            }
        }

        public void Init()
        {
            _eventBus = SL.Current.Get<GenericEventBus<TBaseEvent>>();

            SubscribeToEvents();
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

        private void HandlePauseEvent(ref PauseEvent eventdata)
        {
            OpenMenu();
        }

        private void HandleResumeEvent(ref ResumeEvent eventdata)
        {
            Resume();
        }

        public void Dispose()
        {
        }

        public void BeforeDispose()
        {
        }

        private void OpenMenu()
        {
            if (!_hidden)
                return;

            _hidden = false;
            _uiMenu.RemoveFromClassList(Hidden);
        }

        private void Resume()
        {
            if (_hidden)
                return;

            _hidden = true;
            _uiMenu.AddToClassList(Hidden);
        }
    }

}