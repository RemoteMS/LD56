using System;
using EventBus.Events;
using EventBus.Events.UI;
using EventBus.Events.UI.AudioSettings;
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

        private Slider _masterSlider;
        private Slider _ambientSlider;
        private Slider _sfxSlider;
        private Slider _sensitivitySlider;

        private VisualElement _root;

        private bool _hidden = true;

        private VisualElement _uiMenu;
        private GenericEventBus<TBaseEvent> _eventBus;

        private const string UIMenu = "ui-menu";
        private const string Hidden = "hidden";

        private const string ResumeButton = "resume-button";
        private const string SettingsButton = "settings-button";
        private const string ExitButton = "exit-button";
        private const string MasterSlider = "master-slider";
        private const string AmbientSlider = "ambient-slider";
        private const string SfxSlider = "sfx-slider";
        private const string SensitivitySlider = "sensitivity-slider";


        private void OnEnable()
        {
            _root = uiDocument.rootVisualElement;

            _uiMenu = _root.Q<VisualElement>(UIMenu);


            _resumeButton = _root.Q<Button>(ResumeButton);
            _settingsButton = _root.Q<Button>(SettingsButton);
            _exitButton = _root.Q<Button>(ExitButton);

            _resumeButton.clicked += OnResumeClicked;
            _settingsButton.clicked += OnSettingsClicked;
            _exitButton.clicked += OnExitClicked;


            _masterSlider = _root.Q<Slider>(MasterSlider);
            _ambientSlider = _root.Q<Slider>(AmbientSlider);
            _sfxSlider = _root.Q<Slider>(SfxSlider);
            _sensitivitySlider = _root.Q<Slider>(SensitivitySlider);

            _masterSlider.RegisterValueChangedCallback(evt => OnSliderValueChanged(evt,      "Master"));
            _ambientSlider.RegisterValueChangedCallback(evt => OnSliderValueChanged(evt,     "Ambient"));
            _sfxSlider.RegisterValueChangedCallback(evt => OnSliderValueChanged(evt,         "SFX"));
            _sensitivitySlider.RegisterValueChangedCallback(evt => OnSliderValueChanged(evt, "Mouse Sensitivity"));
        }

        private void OnDisable()
        {
            _resumeButton.clicked -= OnResumeClicked;
            _settingsButton.clicked -= OnSettingsClicked;
            _exitButton.clicked -= OnExitClicked;

            _masterSlider.UnregisterValueChangedCallback(evt => OnSliderValueChanged(evt,      "Master"));
            _ambientSlider.UnregisterValueChangedCallback(evt => OnSliderValueChanged(evt,     "Ambient"));
            _sfxSlider.UnregisterValueChangedCallback(evt => OnSliderValueChanged(evt,         "SFX"));
            _sensitivitySlider.UnregisterValueChangedCallback(evt => OnSliderValueChanged(evt, "Mouse Sensitivity"));

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
            _eventBus.Raise<ExitGameEvent>(new());
            Debug.Log("Exit button clicked!");
        }


        private void OnSliderValueChanged(ChangeEvent<float> evt, string sliderName)
        {
            Debug.Log($"{sliderName} slider value changed to: {evt.newValue}");
            HandleSliderData(sliderName, evt.newValue);
        }

        private float MasterVal => _masterSlider.value;
        private MasterEvent _masterEvent = new();

        private float AmbientVal => _ambientSlider.value;
        private AmbientEvent _ambientEvent = new();

        private float SfxVal => _sfxSlider.value;
        private SFXEvent _sfxEvent = new();

        private float MouseVal => _sensitivitySlider.value / 10f;
        private MouseSensitivityEvent _sensitivityEventEvent = new();


        private void HandleSliderData(string sliderName, float value)
        {
            switch (sliderName)
            {
                case "Master":
                    _masterEvent.Value = MasterVal;
                    _eventBus.Raise<MasterEvent>(_masterEvent);

                    Debug.Log($"Adjusting Master volume to: {value}");
                    break;

                case "Ambient":
                    _ambientEvent.Value = AmbientVal;
                    _eventBus.Raise<AmbientEvent>(_ambientEvent);
                    Debug.Log($"Adjusting Ambient sound to: {value}");
                    break;

                case "SFX":
                    _sfxEvent.Value = SfxVal;
                    _eventBus.Raise<SFXEvent>(_sfxEvent);
                    Debug.Log($"Adjusting SFX sound to: {value}");
                    break;

                case "Mouse Sensitivity":
                    _sensitivityEventEvent.Value = MouseVal;
                    _eventBus.Raise<MouseSensitivityEvent>(_sensitivityEventEvent);
                    Debug.Log($"Adjusting Mouse sensitivity to: {value}");
                    break;

                default:
                    Debug.Log("Unknown slider");
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