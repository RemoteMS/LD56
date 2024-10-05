using System;
using EventBus.Events;
using GenericEventBus;
using Helpers.Interfaces;
using ServiceLocator;
using UnityEngine;

namespace Player
{
    public class PlayerController
        : IService,
            IInitializable,
            IDisposable,
            IGameEventListener,
            IGameUpdatable,
            IGameLateUpdatable
    {
        public PlayerController()
        {
        }

        private GenericEventBus<TBaseEvent> _eventBus;
        private PlayerSettings _playerSettings;

        private PlayerMover _playerMover;
        private PlayerWatcher _playerWatcher;

        public void Init()
        {
            _eventBus = SL.Current.Get<GenericEventBus<TBaseEvent>>();
            _playerSettings = SL.Current.Get<PlayerSettings>();

            _playerMover = new PlayerMover(_playerSettings);
            _playerWatcher = new PlayerWatcher(_playerSettings);

            SubscribeToEvents();
        }

        public void OnLateUpdateTrigger()
        {
        }

        public void OnUpdateTrigger()
        {
            _playerMover.Move();
        }

        public void SubscribeToEvents()
        {
            _eventBus.SubscribeTo<MoveEvent>(HandleMoveEvent);
            _eventBus.SubscribeTo<SprintEvent>(HandleSprintEvent);
            _eventBus.SubscribeTo<CancelSprintEvent>(HandleCancelSprintEvent);
            _eventBus.SubscribeTo<LookEvent>(HandleLookEventEvent);
        }

        public void UnsubscribeFromEvents()
        {
            _eventBus.UnsubscribeFrom<MoveEvent>(HandleMoveEvent);
            _eventBus.UnsubscribeFrom<SprintEvent>(HandleSprintEvent);
            _eventBus.UnsubscribeFrom<CancelSprintEvent>(HandleCancelSprintEvent);
            _eventBus.UnsubscribeFrom<LookEvent>(HandleLookEventEvent);
        }

        private void HandleLookEventEvent(ref LookEvent eventdata)
        {
            _playerWatcher.Look(eventdata.LookDirection);
            Debug.Log($"- OnLook - {eventdata.LookDirection}");
        }

        private void HandleCancelSprintEvent(ref CancelSprintEvent eventdata)
        {
            _playerMover.SetSprint(false);
        }

        private void HandleSprintEvent(ref SprintEvent eventdata)
        {
            _playerMover.SetSprint(true);
        }

        private void HandleMoveEvent(ref MoveEvent eventdata)
        {
            _playerMover.SetDirection(eventdata.Direction);
        }

        public void Dispose()
        {
            UnsubscribeFromEvents();
        }
    }

    public class PlayerWatcher
    {
        private readonly PlayerSettings _playerSettings;

        private const float MinLimit = 0f;
        private const float MaxLimit = 180f;

        private float _xRotation = 0f;

        public PlayerWatcher(PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
        }

        public void Look(Vector2 eventdataLookDirection)
        {
            var mouseX = eventdataLookDirection.x * _playerSettings.mouseSensitivity;
            var mouseY = eventdataLookDirection.y * _playerSettings.mouseSensitivity;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _playerSettings.playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

            _playerSettings.playerTransform.Rotate(Vector3.up * mouseX);
        }
    }

    public class PlayerMover
    {
        private PlayerSettings _playerSettings;

        private CharacterController _characterController;

        public bool IsMoving => _moveDirection != Vector2.zero;
        private Vector2 _moveDirection = Vector2.zero;

        private bool _isSprint = false;

        public PlayerMover(PlayerSettings playerSettings)
        {
            _characterController = playerSettings.characterController;
            _playerSettings = playerSettings;
        }

        public void Move()
        {
            if (IsMoving)
            {
                var speed = _isSprint ? _playerSettings.sprintSpeed : _playerSettings.walkSpeed;

                var direction =
                    _playerSettings.playerTransform.TransformDirection(_moveDirection.x, 0,
                        _moveDirection.y);

                _characterController.Move(direction * (speed * Time.deltaTime));
            }
        }


        public void SetDirection(Vector2 eventdataDirection)
        {
            _moveDirection = eventdataDirection;
        }

        public void SetSprint(bool isSprint)
        {
            _isSprint = isSprint;
        }

    }
}