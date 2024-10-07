using System;
using Common.AI.GameMaster;
using Common.GameGontrollers;
using Common.TMP;
using EventBus.Events;
using GenericEventBus;
using Helpers.Interfaces;
using Inputs;
using Player;
using ServiceLocator.Reflection;
using UI;
using UnityEngine;

namespace ServiceLocator
{
    // crutch
    [DefaultExecutionOrder(-1000)]
    public class ServicesControllerGO : ServicesControllerBase
    {

        #region Inputs

        [Header("Inputs Actions"), SerializeField] [RegisterField(typeof(IDisposable))]
        private InputReader _inputReader;

        #endregion


        [RegisterField(typeof(IDisposable), typeof(IBeforeDisposable))]
        private AppController _appController;


        [RegisterField(typeof(IDisposable))] private GenericEventBus<TBaseEvent> _eventBus;


        #region Player Controls

        [SerializeField] private PlayerSettings playerSettings;

        [RegisterField(typeof(IDisposable), typeof(IGameUpdatable), typeof(IGameLateUpdatable))]
        private PlayerController _playerController;

        #endregion

        [RegisterField(typeof(IDisposable), typeof(IGameUpdatable))]
        private GM _gameMaster;


        [SerializeField] private GMSettings gmSettings;
        [SerializeField] private UIMenuHandler uiMenuHandler;


        private void Awake()
        {
            _eventBus = new GenericEventBus<TBaseEvent>();

            _inputReader = new InputReader();
            _appController = new AppController();
            _playerController = new PlayerController();
            _gameMaster = new GM();

            RegisterToCollection(this);
            RegisterServices();
            Init();
        }

        private void RegisterSettings()
        {
            SL.Current.Register(playerSettings);
            SL.Current.Register(gmSettings);
        }

        private void RegisterServices()
        {
            SL.Initialize();

            SL.Current.Register(_eventBus);

            RegisterSettings();

            Debug.Log("_gameMaster registered");
            SL.Current.Register(_gameMaster);
            SL.Current.Register(_inputReader);
            SL.Current.Register(_appController);
            SL.Current.Register(_playerController);
        }

        private void Init()
        {
            _inputReader.Init();
            _playerController.Init();
            _appController.Init();
            _gameMaster.Init();


            uiMenuHandler.Init();
        }


        public void StartGame()
        {
        }

        public void ToMenu()
        {
        }
    }
}