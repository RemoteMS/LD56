using System;
using EventBus.Events;
using GenericEventBus;
using Inputs;
using ServiceLocator.Reflection;
using UnityEngine;

namespace ServiceLocator
{
    [DefaultExecutionOrder(-100)]
    public class ServicesControllerGO : ServicesControllerBase
    {
        #region  Inputs
        [Header("Inputs Actions"), SerializeField]
        [RegisterField(typeof(IDisposable))]
        private InputReader _inputReader;
        #endregion

        [RegisterField(typeof(IDisposable))]
        private GenericEventBus<TBaseEvent> _eventBus;

        private void Awake()
        {
            _eventBus = new GenericEventBus<TBaseEvent>();

            _inputReader = new InputReader();

            RegisterToCollection(this);
            RegisterServices();
            Init();
        }

        private void RegisterSettings()
        {
            // SL.Current.Register(_inputReaderSettings);
        }

        private void RegisterServices()
        {
            SL.Initialize();

            SL.Current.Register(_eventBus);

            RegisterSettings();

            SL.Current.Register(_inputReader);
        }

        private void Init()
        {
            _inputReader.Init();
        }
    }
}
